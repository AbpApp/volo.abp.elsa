using Elsa.Extensions;
using Elsa.Mapping;
using Elsa.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Elsa;
using Volo.Abp.Modularity;

namespace Volo.Abp.WorkFlowManagement
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpElsaModule),
        typeof(WorkFlowManagementDomainSharedModule)
    )]
    public class WorkFlowManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<WorkFlowManagementDomainModule>();
            context.Services
                .AddScoped<IWorkflowInstanceStore, DatabaseWorkflowInstanceStore>();
            context.Services.AddNotificationHandlers(typeof(AbpLiquidContextHandler));
            context.Services
                .AddScoped<IWorkflowDefinitionStore, DatabaseWorkflowDefinitionStore>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<WorkFlowManagementDomainAutoMapperProfile>(validate: false);
                options.AddMaps<NodaTimeProfile>(validate: false);
            });
        }
    }
}
