using Elsa.Models;
using Elsa.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Extensions;
using ElsaWorkflowDefinitionVersion = Elsa.Models.WorkflowDefinitionVersion;
namespace Volo.Abp.WorkFlowManagement
{
    public class WorkflowDefinitionAppService : WorkFlowManagementAppService, IWorkflowDefinitionAppService
    {
        private readonly IWorkflowDefinitionStore _workflowDefinitionStore;
        private readonly IWorkflowDefinitionVersionRepository _workflowDefinitionVersionRepository;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        public WorkflowDefinitionAppService(IWorkflowDefinitionStore workflowDefinitionStore, IWorkflowInstanceStore workflowInstanceStore, IWorkflowDefinitionVersionRepository workflowDefinitionVersionRepository)
        {
            _workflowDefinitionStore = workflowDefinitionStore;
            _workflowDefinitionVersionRepository = workflowDefinitionVersionRepository;
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
                x => new ConnectionDefinition(GuidGenerator.Create().ToString(), x.SourceActivityId, x.DestinationActivityId, x.Outcome)).ToList();
            workflow.IsLatest = true;
            await _workflowDefinitionVersionRepository.InsertAsync(workflow);
        }

        public async Task DeleteAsync(string id)
        {
            await _workflowDefinitionStore.DeleteAsync(id);
        }

        public async Task<WorkflowDefinitionDto> GetAsync(string id)
        {
            var workflowDefinition = await _workflowDefinitionVersionRepository.GetByIdAsync(id,true);
            return ObjectMapper.Map<WorkflowDefinitionVersion, WorkflowDefinitionDto>(workflowDefinition);
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

        public async Task UpdateAsync(string id, WorkflowDefinitionUpdateDto input)
        {
            var workflowDefinition =await _workflowDefinitionVersionRepository.GetAsync(id);
            if (workflowDefinition == null)
                throw new UserFriendlyException("未找到工作流定义！");
            workflowDefinition.Activities = input.Activities
                .Select(x => new ActivityDefinition(x.Id, x.Type, x.State, x.Left, x.Top))
                .ToList();

            workflowDefinition.Connections = input.Connections.Select(
                x => new ConnectionDefinition(GuidGenerator.Create().ToString(), x.SourceActivityId, x.DestinationActivityId, x.Outcome)).ToList();
            workflowDefinition.Description = input.Description;
            workflowDefinition.Name = input.Name;
            workflowDefinition.IsDisabled = input.IsDisabled;
            workflowDefinition.IsSingleton = input.IsSingleton;

            await _workflowDefinitionVersionRepository.UpdateAsync(workflowDefinition);
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
