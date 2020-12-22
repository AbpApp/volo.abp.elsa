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
            builder.Entity<WorkflowDefinitionVersion>(b =>
            {
                b.ToTable(options.TablePrefix + "WorkflowDefinitionVersions", options.Schema);
                b.ConfigureFullAuditedAggregateRoot();
                b.Property(x => x.DefinitionId);
                b.HasMany(x => x.Activities).WithOne(x => x.WorkflowDefinitionVersion);
                b.HasMany(x => x.Connections).WithOne(x => x.WorkflowDefinitionVersion);
                b.Property(x => x.Variables).HasConversion(x => Serialize(x), x => Deserialize<Variables>(x));
            });
            builder.Entity<WorkflowInstance>(b =>
            {
                b.ToTable(options.TablePrefix + "WorkflowInstances", options.Schema);
                b.ConfigureFullAuditedAggregateRoot();
                b.Property(x => x.Status).HasConversion<string>();
                b
                    .Property(x => x.Scope)
                    .HasConversion(
                        x => Serialize(x),
                        x => Deserialize<WorkflowExecutionScope>(x)
                    );
                b
                    .Property(x => x.ExecutionLog)
                    .HasConversion(
                        x => Serialize(x),
                        x => Deserialize<ICollection<LogEntry>>(x)
                    );

                b
                    .Property(x => x.Fault)
                    .HasConversion(
                        x => Serialize(x),
                        x => Deserialize<WorkflowFault>(x)
                    );

                b
                    .Property(x => x.Input)
                    .HasConversion(
                        x => Serialize(x),
                        x => Deserialize<Variables>(x)
                    );
                b
                    .HasMany(x => x.Activities)
                    .WithOne(x => x.WorkflowInstance);
            
                b
                    .HasMany(x => x.BlockingActivities)
                    .WithOne(x => x.WorkflowInstance);
            });
            builder.Entity<ActivityDefinition>(b =>
            {
                b.ToTable(options.TablePrefix + "ActivityDefinitions", options.Schema);
                b.ConfigureByConvention();
                b
                    .Property(x => x.State)
                    .HasConversion(x => Serialize(x), x => Deserialize<JObject>(x));
            });
            builder.Entity<ConnectionDefinition>(b =>
            {
                b.ToTable(options.TablePrefix + "ConnectionDefinitions", options.Schema);
                b.HasKey(c => new { c.DestinationActivityId, c.SourceActivityId });
                b.HasIndex(c => new  { c.DestinationActivityId, c.SourceActivityId });

            });
            builder.Entity<ActivityInstance>(b =>
            {
                b.ToTable(options.TablePrefix + "ActivityInstances", options.Schema);
                b.ConfigureByConvention();
                b
                    .Property(x => x.State)
                    .HasConversion(x => Serialize(x), x => Deserialize<JObject>(x));
                b
                    .Property(x => x.Output)
                    .HasConversion(x => Serialize(x), x => Deserialize<JObject>(x));
            });
            builder.Entity<BlockingActivity>(b =>
            {
                b.ToTable(options.TablePrefix + "BlockingActivitys", options.Schema);
                b.ConfigureByConvention();
                b.HasKey(x => x.Id);
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