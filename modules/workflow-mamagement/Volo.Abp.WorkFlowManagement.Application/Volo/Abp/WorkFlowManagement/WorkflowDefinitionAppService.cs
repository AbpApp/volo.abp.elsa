using Elsa.Models;
using Elsa.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.WorkFlowManagement
{
    public class WorkflowDefinitionAppService : WorkFlowManagementAppService, IWorkflowDefinitionAppService
    {
        private readonly IWorkflowDefinitionStore _workflowDefinitionStore;

        public WorkflowDefinitionAppService(IWorkflowDefinitionStore workflowDefinitionStore)
        {
            _workflowDefinitionStore = workflowDefinitionStore;
        }
    }
}
