using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Volo.Abp.WorkFlowManagement
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(WorkFlowManagementDomainSharedModule)
    )]
    public class WorkFlowManagementDomainModule : AbpModule
    {

    }
}
