using Elsa.Metadata;
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
    [ControllerName("Activity")]
    [Route("api/workflow-management/activity")]
    public class ActivityDefinitionController : WorkFlowManagementController, IActivityDefinitionAppService
    {
        private readonly IActivityDefinitionAppService _activityDefinitionAppService;

        public ActivityDefinitionController(IActivityDefinitionAppService activityDefinitionAppService)
        {
            _activityDefinitionAppService = activityDefinitionAppService;
        }
        [HttpGet]
        public ActivityDescriptor[] GetActivityDescriptors()
        {
           return _activityDefinitionAppService.GetActivityDescriptors();
        }
    }
}
