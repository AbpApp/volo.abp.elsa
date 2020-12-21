using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.WorkFlowManagement.EntityFrameworkCore
{
    [ConnectionStringName(WorkFlowManagementDbProperties.ConnectionStringName)]
    public interface IWorkFlowManagementDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}