using Volo.Abp.WorkFlowManagement.Localization;
using Volo.Abp.Application.Services;

namespace Volo.Abp.WorkFlowManagement
{
    public abstract class WorkFlowManagementAppService : ApplicationService
    {
        protected WorkFlowManagementAppService()
        {
            LocalizationResource = typeof(WorkFlowManagementResource);
            ObjectMapperContext = typeof(WorkFlowManagementApplicationModule);
        }
    }
}
