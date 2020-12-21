using System;
using System.Collections.Generic;
using Elsa.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.WorkFlowManagement.EntityFrameworkCore
{
    public static class WorkFlowManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureWorkFlowManagement(
            this ModelBuilder builder,
            Action<WorkFlowManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new WorkFlowManagementModelBuilderConfigurationOptions(
                WorkFlowManagementDbProperties.DbTablePrefix,
                WorkFlowManagementDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);
            builder.Entity<WorkflowDefinitionVersion>(options =>
            {
                options.ConfigureFullAudited();
                options.Property(x => x.DefinitionId);
                options.HasMany(x => x.Activities).WithOne(x => x.WorkflowDefinitionVersion);
                options.HasMany(x => x.Connections).WithOne(x => x.WorkflowDefinitionVersion);
                options.Property(x => x.Variables).HasConversion(x => Serialize(x), x => Deserialize<Variables>(x));
            });
            builder.Entity<WorkflowInstance>(options =>
            {
                options.ConfigureFullAudited();
                options.Property(x => x.Status).HasConversion<string>();
                options
                    .Property(x => x.Scope)
                    .HasConversion(
                        x => Serialize(x),
                        x => Deserialize<WorkflowExecutionScope>(x)
                    );
                options
                    .Property(x => x.ExecutionLog)
                    .HasConversion(
                        x => Serialize(x),
                        x => Deserialize<ICollection<LogEntry>>(x)
                    );

                options
                    .Property(x => x.Fault)
                    .HasConversion(
                        x => Serialize(x),
                        x => Deserialize<WorkflowFault>(x)
                    );

                options
                    .Property(x => x.Input)
                    .HasConversion(
                        x => Serialize(x),
                        x => Deserialize<Variables>(x)
                    );
                options
                    .HasMany(x => x.Activities)
                    .WithOne(x => x.WorkflowInstance);
            
                options
                    .HasMany(x => x.BlockingActivities)
                    .WithOne(x => x.WorkflowInstance);
            });
            builder.Entity<ActivityDefinition>(options =>
            {
                options
                    .Property(x => x.State)
                    .HasConversion(x => Serialize(x), x => Deserialize<JObject>(x));
            });
            builder.Entity<ConnectionDefinition>(options =>
            {
                options.ConfigureByConvention();
            });
            builder.Entity<ActivityInstance>(options =>
            {
                options.ConfigureByConvention();
                options
                    .Property(x => x.State)
                    .HasConversion(x => Serialize(x), x => Deserialize<JObject>(x));
                options
                    .Property(x => x.Output)
                    .HasConversion(x => Serialize(x), x => Deserialize<JObject>(x));
            });
            builder.Entity<BlockingActivity>(options =>
            {
                options.ConfigureByConvention();
                options.HasKey(x => x.Id);
            });
        }
        private static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        private static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        } 
    }
    
}