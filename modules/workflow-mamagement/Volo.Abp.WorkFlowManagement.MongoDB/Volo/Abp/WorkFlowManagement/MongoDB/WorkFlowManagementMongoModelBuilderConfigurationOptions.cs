using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.WorkFlowManagement.MongoDB
{
    public class WorkFlowManagementMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public WorkFlowManagementMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}