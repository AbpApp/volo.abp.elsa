using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.WorkFlowManagement.EntityFrameworkCore
{
    public class WorkFlowManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public WorkFlowManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}