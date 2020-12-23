using Elsa;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Serialization.Formatters;
using Elsa.Services;
using Elsa.Services.Models;
using Elsa.WorkflowEventHandlers;
using System;
using System.Collections.Generic;
using Volo.Abp.Elsa;
using Elsa.WorkflowProviders;
using Elsa.Mapping;
using Elsa.WorkflowBuilders;
using Elsa.Extensions;
using Elsa.Activities;
using MediatR;
using Elsa.Persistence;
using Elsa.Caching;
using Elsa.Persistence.Memory;
using Elsa.Runtime;
using Elsa.Scripting.Liquid.Extensions;
using Elsa.Activities.Workflows.Activities;
using Elsa.Activities.ControlFlow.Extensions;
using Elsa.Activities.UserTask.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionElsaExtensions
    {
        public static IServiceCollection AddAbpElsa(
            this IServiceCollection services,
            Action<ElsaBuilder> configure = null)
        {
            return services
                .AddElsaWorkFlow(configure)
                .AddStartupRunner()
                .AddJavaScriptExpressionEvaluator()
                .AddLiquidExpressionEvaluator()
                .AddControlFlowActivities()
                .AddWorkflowActivities()
                .AddUserTaskActivities();
        }
        public static IServiceCollection AddElsaWorkFlow(this IServiceCollection services, Action<ElsaBuilder> configure = null)
        {
            var configuration = new ElsaBuilder(services);
            services
                .AddTransient<Func<IEnumerable<IActivity>>>(sp => sp.GetServices<IActivity>)
                .AddSingleton<IIdGenerator, ElsaIdGenerator>()
                .AddSingleton<IWorkflowSerializer, WorkflowSerializer>()
                .TryAddProvider<ITokenFormatter, JsonTokenFormatter>(ServiceLifetime.Singleton)
                .TryAddProvider<ITokenFormatter, YamlTokenFormatter>(ServiceLifetime.Singleton)
                .TryAddProvider<ITokenFormatter, XmlTokenFormatter>(ServiceLifetime.Singleton)
                .TryAddProvider<IExpressionEvaluator, LiteralEvaluator>(ServiceLifetime.Singleton)
                .AddTransient<IWorkflowFactory, WorkflowFactory>()
                .AddScoped<IWorkflowExpressionEvaluator, WorkflowExpressionEvaluator>()
                .AddSingleton<IWorkflowSerializerProvider, WorkflowSerializerProvider>()
                .AddTransient<IWorkflowRegistry, WorkFlowManager>()
                .AddScoped<IWorkflowEventHandler, PersistenceWorkflowEventHandler>()
                .AddScoped<IWorkflowInvoker, WorkFlowInvoker>()
                .AddScoped<IActivityResolver, ActivityResolver>()
                .AddScoped<IWorkflowEventHandler, ActivityLoggingWorkflowEventHandler>()
                .AddTransient<IWorkflowProvider, StoreWorkflowProvider>()
                .AddTransient<IWorkflowProvider, CodeWorkflowProvider>()
                .AddTransient<IWorkflowBuilder, WorkflowBuilder>()
                .AddTransient<Func<IWorkflowBuilder>>(sp => sp.GetRequiredService<IWorkflowBuilder>)
                .AddMapperProfile<WorkflowDefinitionProfile>(ServiceLifetime.Singleton)
                .AddActivity<SetVariable>();
            services.AddMediatR(
                mediatr => mediatr.AsSingleton(),
                typeof(ElsaServiceCollectionExtensions));
            configure?.Invoke(configuration);
            EnsurePersistence(configuration);
            EnsureCaching(configuration);
            return services;
        }
        private static IServiceCollection AddWorkflowActivities(this IServiceCollection services)
        {
            return services
                .AddActivity<TriggerWorkflow>()
                .AddActivity<Correlate>()
                .AddActivity<Signaled>()
                .AddActivity<TriggerSignal>()
                .AddActivity<Start>()
                .AddActivity<Finish>();
        }
        private static void EnsurePersistence(ElsaBuilder configuration)
        {
            var hasDefinitionStore = configuration.HasService<IWorkflowDefinitionStore>();
            var hasInstanceStore = configuration.HasService<IWorkflowInstanceStore>();

            if (!hasDefinitionStore || !hasInstanceStore)
                configuration.WithMemoryStores();

            configuration.Services.Decorate<IWorkflowDefinitionStore, PublishingWorkflowDefinitionStore>();
        }

        private static void EnsureCaching(ElsaBuilder configuration)
        {
            if (!configuration.HasService<ISignal>())
                configuration.Services.AddSingleton<ISignal, Signal>();

            configuration.Services.AddMemoryCache();
        }
    }
}
