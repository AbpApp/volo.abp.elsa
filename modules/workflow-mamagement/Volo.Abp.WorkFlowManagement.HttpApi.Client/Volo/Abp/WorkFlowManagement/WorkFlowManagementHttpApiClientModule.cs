using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.WorkFlowManagement
{
    [DependsOn(
        typeof(WorkFlowManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class WorkFlowManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "WorkFlowManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(WorkFlowManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
