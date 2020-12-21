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

    }
}
