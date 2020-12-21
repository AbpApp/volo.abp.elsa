using Localization.Resources.AbpUi;
using Volo.Abp.WorkFlowManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.WorkFlowManagement
{
    [DependsOn(
        typeof(WorkFlowManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class WorkFlowManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(WorkFlowManagementHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<WorkFlowManagementResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
