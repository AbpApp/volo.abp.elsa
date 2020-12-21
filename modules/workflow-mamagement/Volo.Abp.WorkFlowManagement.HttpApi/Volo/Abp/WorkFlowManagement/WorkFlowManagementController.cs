using Volo.Abp.WorkFlowManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.WorkFlowManagement
{
    public abstract class WorkFlowManagementController : AbpController
    {
        protected WorkFlowManagementController()
        {
            LocalizationResource = typeof(WorkFlowManagementResource);
        }
    }
}
