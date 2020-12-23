using Elsa.Metadata;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace Volo.Abp.Elsa
{
    [DependsOn(
        typeof(AbpGuidsModule))]
    [DependsOn(
        typeof(AbpTimingModule))]
    public class AbpElsaModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<IActivityManager, ActivityManager>();
            context.Services.AddScoped<IActivityInvoker, ActivityInvoker>();
        }
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AutoAddWorkFlowDefinitions(context.Services);
        }
        private static void AutoAddWorkFlowDefinitions(IServiceCollection services)
        {
            var activityDefinitionList= new ActivityDefinitionList();
            var definitionProviders = new List<Type>();
            services.OnRegistred(context =>
            {
                if (typeof(IActivity).IsAssignableFrom(context.ImplementationType))
                {
                    activityDefinitionList.Add(ActivityDescriber.Describe(context.ImplementationType));
                }

                if (typeof(IWorkflowProvider).IsAssignableFrom(context.ImplementationType)) {
                    definitionProviders.Add(context.ImplementationType);
                }
            });
            services.Configure<AbpElsaOptions>(options =>
            {
                options.ActivityDefinitions = activityDefinitionList;
                options.DefinitionProviders.AddIfNotContains(definitionProviders);
            });
        }
    }
}
