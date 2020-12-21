using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Volo.Abp.WorkFlowManagement
{
    [DependsOn(
        typeof(WorkFlowManagementDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class WorkFlowManagementApplicationContractsModule : AbpModule
    {

    }
}
