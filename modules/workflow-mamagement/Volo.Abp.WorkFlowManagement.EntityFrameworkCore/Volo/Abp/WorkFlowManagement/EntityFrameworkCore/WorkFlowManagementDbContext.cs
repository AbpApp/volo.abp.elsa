using Elsa.Models;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.WorkFlowManagement.EntityFrameworkCore
{
    [ConnectionStringName(WorkFlowManagementDbProperties.ConnectionStringName)]
    public class WorkFlowManagementDbContext : AbpDbContext<WorkFlowManagementDbContext>, IWorkFlowManagementDbContext
    {

        public DbSet<WorkflowDefinitionVersion> WorkflowDefinitionVersions { get; set; }
        public DbSet<WorkflowInstance> WorkflowInstances { get; set; }
        public DbSet<ActivityDefinition> ActivityDefinitions { get; set; }
        public DbSet<ConnectionDefinition> ConnectionDefinitions { get; set; }
        public DbSet<ActivityInstance> ActivityInstances { get; set; }
        public DbSet<BlockingActivity> BlockingActivities { get; set; }
        public WorkFlowManagementDbContext(DbContextOptions<WorkFlowManagementDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureWorkFlowManagement();
        }
    }
}