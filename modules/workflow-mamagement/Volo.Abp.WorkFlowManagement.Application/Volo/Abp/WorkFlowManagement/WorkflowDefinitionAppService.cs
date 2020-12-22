using Elsa.Models;
using Elsa.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Extensions;
using Elsa.Serialization;
using ElsaWorkflowDefinitionVersion = Elsa.Models.WorkflowDefinitionVersion;
namespace Volo.Abp.WorkFlowManagement
{
    public class WorkflowDefinitionAppService : WorkFlowManagementAppService, IWorkflowDefinitionAppService
    {
        private readonly IWorkflowDefinitionStore _workflowDefinitionStore;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowDefinitionStore _store;
        public WorkflowDefinitionAppService(IWorkflowDefinitionStore workflowDefinitionStore,  IWorkflowDefinitionStore store, IWorkflowInstanceStore workflowInstanceStore)
        {
            _workflowDefinitionStore = workflowDefinitionStore;
            _store = store;
            _workflowInstanceStore = workflowInstanceStore;
        }

        public async Task CreateAsync(WorkflowDefinitionCreateDto input)
        {
            var workflow = new WorkflowDefinitionVersion(
                GuidGenerator.Create().ToString(),
                input.Name,
                input.Description,
                input.IsSingleton,
                input.IsDisabled);
            workflow.Activities = input.Activities
                .Select(x => new ActivityDefinition(x.Id,x.Type, 
                    x.State,
                    x.Left, x.Top))
                .ToList();       
            workflow.Connections = input.Connections.Select(
                x => new ConnectionDefinition(x.SourceActivityId, x.DestinationActivityId, x.Outcome)).ToList();
            var publishedDefinition = await _store.GetByIdAsync(
                workflow.DefinitionId,
                VersionOptions.Published);
            
            if (publishedDefinition != null)
            {
                publishedDefinition.IsPublished = false;
                publishedDefinition.IsLatest = false;
                await _store.UpdateAsync(publishedDefinition);
            }
            if (workflow.IsPublished)
            {
                workflow.Version++;
            }
            else
            {
                workflow.IsPublished = true;   
            }
            workflow.IsLatest = true;
            await _store.SaveAsync(Map(workflow));
        }

        public async Task DeleteAsync(string id)
        {
            await _workflowDefinitionStore.DeleteAsync(id);
        }

        public  async Task<List<WorkflowDefinitionListDto>> GetListAsync()
        {
            var workflows = await _workflowDefinitionStore.ListAsync(
                VersionOptions.LatestOrPublished
            );
            var result = new List<WorkflowDefinitionListDto>();
            
            foreach (var workflow in workflows)
            {
                var workflowModel = await CreateWorkflowDefinitionListItemModelAsync(workflow);
                result.Add(workflowModel);
            }

            return result;
        }
        private async Task<WorkflowDefinitionListDto> CreateWorkflowDefinitionListItemModelAsync(
            ElsaWorkflowDefinitionVersion workflowDefinition)
        {
            var instances = await _workflowInstanceStore
                .ListByDefinitionAsync(workflowDefinition.DefinitionId)
                .ToListAsync();

            return new WorkflowDefinitionListDto
            {
                WorkflowDefinition = workflowDefinition,
                AbortedCount = instances.Count(x => x.Status == WorkflowStatus.Aborted),
                FaultedCount = instances.Count(x => x.Status == WorkflowStatus.Faulted),
                FinishedCount = instances.Count(x => x.Status == WorkflowStatus.Finished),
                ExecutingCount = instances.Count(x => x.Status == WorkflowStatus.Executing),
            };
        }
        private WorkflowDefinitionVersion Map(ElsaWorkflowDefinitionVersion source) => ObjectMapper.Map<ElsaWorkflowDefinitionVersion,WorkflowDefinitionVersion>(source);
        private ElsaWorkflowDefinitionVersion Map(WorkflowDefinitionVersion source) => ObjectMapper.Map<WorkflowDefinitionVersion,ElsaWorkflowDefinitionVersion>(source);

    }
}
