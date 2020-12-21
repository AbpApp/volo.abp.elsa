using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.WorkFlowManagement.MongoDB
{
    [ConnectionStringName(WorkFlowManagementDbProperties.ConnectionStringName)]
    public interface IWorkFlowManagementMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
