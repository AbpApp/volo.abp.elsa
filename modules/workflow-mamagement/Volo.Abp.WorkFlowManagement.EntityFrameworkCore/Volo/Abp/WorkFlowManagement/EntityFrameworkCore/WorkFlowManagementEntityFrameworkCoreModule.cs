using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.WorkFlowManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(WorkFlowManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class WorkFlowManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<WorkFlowManagementDbContext>(options =>
            {
                options.AddRepository<WorkflowInstance, EfCoreWorkflowInstanceRepository>();
                options.AddRepository<WorkflowDefinitionVersion, EfCoreWorkflowDefinitionVersionRepository>();
                options.AddRepository<ActivityDefinition, EfCoreActivityDefinitionRepository>();
                options.AddRepository<ActivityInstance, EfCoreActivityInstanceRepository>();
            });
        }
    }
}