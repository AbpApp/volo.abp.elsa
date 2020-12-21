using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.WorkFlowManagement.MongoDB
{
    [ConnectionStringName(WorkFlowManagementDbProperties.ConnectionStringName)]
    public class WorkFlowManagementMongoDbContext : AbpMongoDbContext, IWorkFlowManagementMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureWorkFlowManagement();
        }
    }
}