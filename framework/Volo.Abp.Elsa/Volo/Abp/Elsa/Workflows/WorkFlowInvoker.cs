using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Extensions;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Results;
using Elsa.Services;
using Elsa.Services.Extensions;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NodaTime;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.Elsa
{
   public class WorkFlowInvoker: IWorkflowInvoker
    {
        private readonly IActivityInvoker _activityInvoker;
        private readonly IWorkflowFactory _workflowFactory;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IEnumerable<IWorkflowEventHandler> _workflowEventHandlers;
        private readonly IClock _clock;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WorkFlowInvoker> _logger;
        private readonly IDistributedEventBus _distributedEventBus;

        public WorkFlowInvoker(
            IActivityInvoker activityInvoker,
            IWorkflowFactory workflowFactory,
            IWorkflowRegistry workflowRegistry,
            IWorkflowInstanceStore workflowInstanceStore,
            IEnumerable<IWorkflowEventHandler> workflowEventHandlers,
            IClock clock,
            IServiceProvider serviceProvider,
            ILogger<WorkFlowInvoker> logger, IDistributedEventBus distributedEventBus)
        {
            _activityInvoker = activityInvoker;
            _workflowFactory = workflowFactory;
            _workflowRegistry = workflowRegistry;
            _workflowInstanceStore = workflowInstanceStore;
            _workflowEventHandlers = workflowEventHandlers;
            _clock = clock;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _distributedEventBus = distributedEventBus;
        }

        public Task<WorkflowExecutionContext> StartAsync(
            Workflow workflow,
            IEnumerable<IActivity> startActivities = default,
            CancellationToken cancellationToken = default)
        {
            return ExecuteAsync(workflow, false, startActivities, cancellationToken);
        }

        public Task<WorkflowExecutionContext> StartAsync(
            WorkflowDefinitionVersion workflowDefinition,
            Variables input = default,
            IEnumerable<string> startActivityIds = default,
            string correlationId = default,
            CancellationToken cancellationToken = default)
        {
            var workflow = _workflowFactory.CreateWorkflow(workflowDefinition, input, correlationId: correlationId);
            var startActivities = workflow.Activities.Find(startActivityIds);

            return ExecuteAsync(workflow, false, startActivities, cancellationToken);
        }

        public Task<WorkflowExecutionContext> StartAsync<T>(
            Variables input = default,
            IEnumerable<string> startActivityIds = default,
            string correlationId = default,
            CancellationToken cancellationToken = default) where T : IWorkflow, new()
        {
            var workflow = _workflowFactory.CreateWorkflow<T>(input, correlationId: correlationId);
            var startActivities = workflow.Activities.Find(startActivityIds);

            return ExecuteAsync(workflow, false, startActivities, cancellationToken);
        }

        public Task<WorkflowExecutionContext> ResumeAsync(
            Workflow workflow,
            IEnumerable<IActivity> startActivities = default,
            CancellationToken cancellationToken = default)
        {
            return ExecuteAsync(workflow, true, startActivities, cancellationToken);
        }

        public Task<WorkflowExecutionContext> ResumeAsync<T>(
            WorkflowInstance workflowInstance,
            Variables input = null,
            IEnumerable<string> startActivityIds = default,
            CancellationToken cancellationToken = default) where T : IWorkflow, new()
        {
            var workflow = _workflowFactory.CreateWorkflow<T>(input, workflowInstance);
            return ResumeAsync(workflow, startActivityIds, cancellationToken);
        }

        public async Task<WorkflowExecutionContext> ResumeAsync(
            WorkflowInstance workflowInstance,
            Variables input = null,
            IEnumerable<string> startActivityIds = default,
            CancellationToken cancellationToken = default)
        {
            var definition = await _workflowRegistry.GetWorkflowDefinitionAsync(
                workflowInstance.DefinitionId,
                VersionOptions.SpecificVersion(workflowInstance.Version),
                cancellationToken);

            var workflow = _workflowFactory.CreateWorkflow(definition, input, workflowInstance);
            return await ResumeAsync(workflow, startActivityIds, cancellationToken);
        }

        public Task<WorkflowExecutionContext> ResumeAsync(
            Workflow workflow,
            IEnumerable<string> startActivityIds = default,
            CancellationToken cancellationToken = default)
        {
            
            startActivityIds ??= workflow.BlockingActivities.Select(x => x.Id);
            var startActivities = workflow.Activities.Find(startActivityIds);
            return ExecuteAsync(workflow, true, startActivities, cancellationToken);
        }

        public async Task<IEnumerable<WorkflowExecutionContext>> TriggerAsync(
            string activityType,
            Variables input = default,
            string correlationId = default,
            Func<JObject, bool> activityStatePredicate = default,
            CancellationToken cancellationToken = default)
        {
            var startedExecutionContexts = await StartManyAsync(
                activityType,
                input,
                correlationId,
                activityStatePredicate,
                cancellationToken
            );

            var resumedExecutionContexts = await ResumeManyAsync(
                activityType,
                input,
                correlationId,
                activityStatePredicate,
                cancellationToken
            );

            return startedExecutionContexts.Concat(resumedExecutionContexts);
        }

        private async Task<IEnumerable<WorkflowExecutionContext>> ResumeManyAsync(
            string activityType,
            Variables input = default,
            string correlationId = default,
            Func<JObject, bool> activityStatePredicate = default,
            CancellationToken cancellationToken = default)
        {
            var workflowInstances = await _workflowInstanceStore
                .ListByBlockingActivityAsync(activityType, correlationId, cancellationToken)
                .ToListAsync();

            if (activityStatePredicate != null)
                workflowInstances = workflowInstances.Where(x => activityStatePredicate(x.Item2.State)).ToList();

            return await ResumeManyAsync(
                workflowInstances,
                input,
                cancellationToken
            );
        }

        private async Task<IEnumerable<WorkflowExecutionContext>> StartManyAsync(
            string activityType,
            Variables input = default,
            string correlationId = default,
            Func<JObject, bool> activityStatePredicate = default,
            CancellationToken cancellationToken = default)
        {
            var workflowDefinitions = await _workflowRegistry.ListByStartActivityAsync(activityType, cancellationToken);

            if (activityStatePredicate != null)
                workflowDefinitions = workflowDefinitions.Where(x => activityStatePredicate(x.Item2.State));

            workflowDefinitions = await FilterRunningSingletonsAsync(
                workflowDefinitions,
                cancellationToken
            );

            return await StartManyAsync(workflowDefinitions, input, correlationId, cancellationToken);
        }

        private Task<WorkflowExecutionContext> ExecuteAsync(
            Workflow workflow,
            bool resume,
            IEnumerable<string> startActivityIds = default,
            CancellationToken cancellationToken = default)
        {
            var startActivities = startActivityIds != null
                ? workflow.Activities.Find(startActivityIds)
                : Enumerable.Empty<IActivity>();

            return ExecuteAsync(workflow, resume, startActivities, cancellationToken);
        }

        private async Task<WorkflowExecutionContext> ExecuteAsync(
            Workflow workflow,
            bool resume,
            IEnumerable<IActivity> startActivities = default,
            CancellationToken cancellationToken = default)
        {
            var workflowExecutionContext = await CreateWorkflowExecutionContextAsync(
                workflow,
                startActivities,
                cancellationToken
            );

            var start = !resume;

            while (workflowExecutionContext.HasScheduledActivities)
            {
                var currentActivity = workflowExecutionContext.PopScheduledActivity();

                var result = start
                    ? await ExecuteActivityAsync(workflowExecutionContext, currentActivity, cancellationToken)
                    : await ResumeActivityAsync(workflowExecutionContext, currentActivity, cancellationToken);

                if (result == null)
                    break;

                await result.ExecuteAsync(this, workflowExecutionContext, cancellationToken);

                workflowExecutionContext.IsFirstPass = false;
                start = true;
            }

            await FinalizeWorkflowExecutionAsync(workflowExecutionContext, cancellationToken);

            return workflowExecutionContext;
        }

        private async Task<IEnumerable<WorkflowExecutionContext>> StartManyAsync(
            IEnumerable<(WorkflowDefinitionVersion, ActivityDefinition)> workflowDefinitions,
            Variables input,
            string correlationId,
            CancellationToken cancellationToken1)
        {
            var executionContexts = new List<WorkflowExecutionContext>();

            foreach (var (workflowDefinition, activityDefinition) in workflowDefinitions)
            {
                var startActivityIds = workflowDefinition.Activities
                    .Where(x => x.Id == activityDefinition.Id)
                    .Select(x => x.Id);

                var workflow = _workflowFactory.CreateWorkflow(workflowDefinition, input, correlationId: correlationId);

                var executionContext = await ExecuteAsync(
                    workflow,
                    false,
                    startActivityIds,
                    cancellationToken1
                );
                executionContexts.Add(executionContext);
            }

            return executionContexts;
        }

        private async Task<IEnumerable<WorkflowExecutionContext>> ResumeManyAsync(
            IEnumerable<(WorkflowInstance, ActivityInstance)> workflowInstances,
            Variables input,
            CancellationToken cancellationToken)
        {
            var executionContexts = new List<WorkflowExecutionContext>();
            var workflowInstanceGroups = workflowInstances.GroupBy(x => x.Item1);

            foreach (var workflowInstanceGroup in workflowInstanceGroups)
            {
                var workflowInstance = workflowInstanceGroup.Key;

                var workflowDefinition = await _workflowRegistry.GetWorkflowDefinitionAsync(
                    workflowInstance.DefinitionId,
                    VersionOptions.SpecificVersion(workflowInstance.Version),
                    cancellationToken
                );

                var workflow = _workflowFactory.CreateWorkflow(workflowDefinition, input, workflowInstance);

                foreach (var activity in workflowInstanceGroup)
                {
                    var executionContext = await ExecuteAsync(
                        workflow,
                        true,
                        new[] { activity.Item2.Id },
                        cancellationToken
                    );

                    executionContexts.Add(executionContext);
                }
            }

            return executionContexts;
        }

        private async Task FinalizeWorkflowExecutionAsync(
            WorkflowExecutionContext workflowExecutionContext,
            CancellationToken cancellationToken)
        {
            if (!workflowExecutionContext.Workflow.BlockingActivities.Any() &&
                workflowExecutionContext.Workflow.IsExecuting())
            {
                workflowExecutionContext.Finish();
            }
            else
            {
                // Notify event handlers that halting activities are about to be executed.
                await _workflowEventHandlers.InvokeAsync(
                    async x => await x.InvokingHaltedActivitiesAsync(workflowExecutionContext, cancellationToken),
                    _logger
                );

                // Invoke Halted event on activity drivers that halted the workflow.
                while (workflowExecutionContext.HasScheduledHaltingActivities)
                {
                    var currentActivity = workflowExecutionContext.PopScheduledHaltingActivity();
                    var result = await ExecuteActivityHaltedAsync(
                        workflowExecutionContext,
                        currentActivity,
                        cancellationToken
                    );

                    await result.ExecuteAsync(this, workflowExecutionContext, cancellationToken);
                }
            }

            // Notify event handlers that workflow execution has ended.
            await _workflowEventHandlers.InvokeAsync(
                async x => await x.WorkflowInvokedAsync(workflowExecutionContext, cancellationToken),
                _logger
            );
        }

        private async Task<ActivityExecutionResult> ExecuteActivityAsync(
            WorkflowExecutionContext workflowContext,
            IActivity activity,
            CancellationToken cancellationToken)
        {
            return await InvokeActivityAsync(
                workflowContext,
                activity,
                async () => await _activityInvoker.ExecuteAsync(workflowContext, activity, cancellationToken),
                cancellationToken
            );
        }

        private async Task<ActivityExecutionResult> ResumeActivityAsync(
            WorkflowExecutionContext workflowContext,
            IActivity activity,
            CancellationToken cancellationToken)
        {
            return await InvokeActivityAsync(
                workflowContext,
                activity,
                async () => await _activityInvoker.ResumeAsync(workflowContext, activity, cancellationToken),
                cancellationToken
            );
        }

        private async Task<ActivityExecutionResult> InvokeActivityAsync(
            WorkflowExecutionContext workflowContext,
            IActivity activity,
            Func<Task<ActivityExecutionResult>> executeAction,
            CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    workflowContext.Workflow.Status = WorkflowStatus.Aborted;
                    workflowContext.Workflow.FinishedAt = _clock.GetCurrentInstant();
                    return null;
                }

                return await executeAction();
            }
            catch (Exception ex)
            {
                FaultWorkflow(workflowContext, activity, ex);
            }

            return null;
        }

        private async Task<ActivityExecutionResult> ExecuteActivityHaltedAsync(
            WorkflowExecutionContext workflowContext,
            IActivity activity,
            CancellationToken cancellationToken)
        {
            return await InvokeActivityAsync(
                workflowContext,
                activity,
                async () => await _activityInvoker.HaltedAsync(workflowContext, activity, cancellationToken),
                cancellationToken
            );
        }

        private void FaultWorkflow(WorkflowExecutionContext workflowContext, IActivity activity, Exception ex)
        {
            _logger.LogError(
                ex,
                "An unhandled error occurred while executing an activity. Putting the workflow in the faulted state."
            );
            workflowContext.Fault(activity, ex);
        }

        private async Task<WorkflowExecutionContext> CreateWorkflowExecutionContextAsync(
            Workflow workflow,
            IEnumerable<IActivity> startActivities,
            CancellationToken cancellationToken)
        {
            var workflowExecutionContext = new WorkflowExecutionContext(workflow, _clock, _serviceProvider);
            var startActivityList = startActivities?.ToList() ?? workflow.GetStartActivities().Take(1).ToList();

            foreach (var startActivity in startActivityList)
            {
                if (await startActivity.CanExecuteAsync(workflowExecutionContext, cancellationToken))
                    workflowExecutionContext.ScheduleActivity(startActivity);
            }

            if (workflowExecutionContext.HasScheduledActivities)
            {
                workflow.BlockingActivities.RemoveWhere(workflowExecutionContext.ScheduledActivities.Contains);

                if (workflowExecutionContext.Workflow.Status == WorkflowStatus.Idle)
                    workflowExecutionContext.Start();
            }

            return workflowExecutionContext;
        }

        private async Task<IEnumerable<(WorkflowDefinitionVersion, ActivityDefinition)>> FilterRunningSingletonsAsync(
            IEnumerable<(WorkflowDefinitionVersion, ActivityDefinition)> workflowDefinitions,
            CancellationToken cancellationToken)
        {
            var definitions = workflowDefinitions.ToList();
            var transients = definitions.Where(x => !x.Item1.IsSingleton).ToList();
            var singletons = definitions.Where(x => x.Item1.IsSingleton).ToList();
            var result = transients.ToList();

            foreach (var definition in singletons)
            {
                var instances = await _workflowInstanceStore.ListByStatusAsync(
                    definition.Item1.DefinitionId,
                    WorkflowStatus.Executing,
                    cancellationToken
                );

                if (!instances.Any())
                {
                    result.Add(definition);
                }
            }

            return result;
        }
    }
}
