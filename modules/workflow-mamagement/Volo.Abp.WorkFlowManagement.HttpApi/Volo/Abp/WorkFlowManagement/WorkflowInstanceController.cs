using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.WorkFlowManagement
{
    [RemoteService]
    [Area("Workflor-Management")]
    [ControllerName("WorkflowInstance")]
    [Route("api/workflow-management/workflowInstance")]
    public class WorkflowInstanceController : WorkFlowManagementController, IWorkflowInstanceAppService
    {
        private readonly IWorkflowInstanceAppService _workflowInstanceAppService;

        public WorkflowInstanceController(IWorkflowInstanceAppService workflowInstanceAppService) {

            _workflowInstanceAppService = workflowInstanceAppService;
        }
        [HttpPost]
        [Route("{id}")]
        public virtual Task CreateByDefinitionIdAsync(string id)
        {
           return  _workflowInstanceAppService.CreateByDefinitionIdAsync(id);
        }
        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(string id)
        {
            return _workflowInstanceAppService.DeleteAsync(id);
        }
        [HttpGet]
        [Route("{id}")]
        public virtual Task<WorkflowInstanceDto> GetAsync(string id)
        {
            return _workflowInstanceAppService.GetAsync(id);
        }
        [HttpGet]
        public virtual Task<List<WorkflowInstanceListDto>> GetListAsync()
        {
            return _workflowInstanceAppService.GetListAsync();
        }
    }
}
