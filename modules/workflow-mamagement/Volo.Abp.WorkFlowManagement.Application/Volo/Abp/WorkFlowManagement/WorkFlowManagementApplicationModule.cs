using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace Volo.Abp.WorkFlowManagement
{
    [DependsOn(
        typeof(WorkFlowManagementDomainModule),
        typeof(WorkFlowManagementApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class WorkFlowManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<WorkFlowManagementApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<WorkFlowManagementApplicationModule>(validate: true);
            });
        }
    }
}
