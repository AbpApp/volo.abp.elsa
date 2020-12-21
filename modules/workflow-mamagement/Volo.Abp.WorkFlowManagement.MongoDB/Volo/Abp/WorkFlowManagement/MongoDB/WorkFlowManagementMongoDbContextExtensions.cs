using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Volo.Abp.WorkFlowManagement.MongoDB
{
    public static class WorkFlowManagementMongoDbContextExtensions
    {
        public static void ConfigureWorkFlowManagement(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new WorkFlowManagementMongoModelBuilderConfigurationOptions(
                WorkFlowManagementDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}