using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.WorkFlowManagement
{
    [RemoteService]
    [Area("Workflor-Management")]
    [ControllerName("WorkflowDefinition")]
    [Route("api/workflow-management/workflowDefinition")]
    public class WorkflowDefinitionController : WorkFlowManagementController, IWorkflowDefinitionAppService
    {
        private readonly IWorkflowDefinitionAppService _workflowDefinitionAppService;

        public WorkflowDefinitionController(IWorkflowDefinitionAppService workflowDefinitionAppService)
        {
            _workflowDefinitionAppService = workflowDefinitionAppService;
        }

        [HttpPost]
        public virtual Task CreateAsync(WorkflowDefinitionCreateDto input)
        {
            return _workflowDefinitionAppService.CreateAsync(input);
        }
        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(string id)
        {
            return _workflowDefinitionAppService.DeleteAsync(id);
        }
        [HttpGet]
        [Route("{id}")]
        public virtual Task<WorkflowDefinitionDto> GetAsync(string id)
        {
            return _workflowDefinitionAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<List<WorkflowDefinitionListDto>> GetListAsync()
        {
            return  _workflowDefinitionAppService.GetListAsync();
        }
        [HttpPut]
        [Route("{id}")]
        public virtual Task UpdateAsync(string id, WorkflowDefinitionUpdateDto input)
        {
            return _workflowDefinitionAppService.UpdateAsync(id, input);
        }
    }
}