using Elsa.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Elsa.Persistence;
using ElsaWorkflowDefinitionVersion = Elsa.Models.WorkflowDefinitionVersion;

namespace Volo.Abp.WorkFlowManagement
{
    public class WorkflowInstanceAppService : WorkFlowManagementAppService, IWorkflowInstanceAppService
    {
        private readonly IWorkflowInvoker _workflowInvoker;
        private readonly IWorkflowDefinitionVersionRepository _workflowDefinitionVersionRepository;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;

        public WorkflowInstanceAppService(IWorkflowInvoker workflowInvoker, IWorkflowDefinitionVersionRepository workflowDefinitionVersionRepository, IWorkflowInstanceRepository workflowInstanceRepository, IWorkflowInstanceStore workflowInstanceStore)
        {
            _workflowInvoker = workflowInvoker;
            _workflowDefinitionVersionRepository = workflowDefinitionVersionRepository;
            _workflowInstanceRepository = workflowInstanceRepository;
            _workflowInstanceStore = workflowInstanceStore;
        }
        public  async Task CreateByDefinitionIdAsync(string id)
        {
            var workDefinitionEntity = await _workflowDefinitionVersionRepository.GetAsync(id);
            var workDefinition = ObjectMapper.Map<WorkflowDefinitionVersion, ElsaWorkflowDefinitionVersion>(workDefinitionEntity);
            await _workflowInvoker.StartAsync(workDefinition);
        }

        public async Task DeleteAsync(string id)
        {
            await _workflowInstanceStore.DeleteAsync(id);
        }

        public async Task<WorkflowInstanceDto> GetAsync(string id)
        {
           var entity= await _workflowInstanceRepository.GetByInstanceIdAsync(id);
           return ObjectMapper.Map<WorkflowInstance, WorkflowInstanceDto>(entity);
        }

        public async Task<List<WorkflowInstanceListDto>> GetListAsync()
        {
            var lists = await _workflowInstanceRepository.GetListAsync();
            return ObjectMapper.Map<List<WorkflowInstance>,List<WorkflowInstanceListDto> >(lists); 
        }
    }
}
