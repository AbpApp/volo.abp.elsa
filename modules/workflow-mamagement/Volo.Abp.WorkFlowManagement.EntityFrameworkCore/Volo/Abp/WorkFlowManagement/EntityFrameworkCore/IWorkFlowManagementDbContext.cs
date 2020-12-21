using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.WorkFlowManagement.EntityFrameworkCore
{
    [ConnectionStringName(WorkFlowManagementDbProperties.ConnectionStringName)]
    public interface IWorkFlowManagementDbContext : IEfCoreDbContext
    {
         DbSet<WorkflowDefinitionVersion> WorkflowDefinitionVersions { get; set; }
         DbSet<WorkflowInstance> WorkflowInstances { get; set; }
         DbSet<ActivityDefinition> ActivityDefinitions { get; set; }
         DbSet<ConnectionDefinition> ConnectionDefinitions { get; set; }
         DbSet<ActivityInstance> ActivityInstances { get; set; }
         DbSet<BlockingActivity> BlockingActivities { get; set; }
    }
}