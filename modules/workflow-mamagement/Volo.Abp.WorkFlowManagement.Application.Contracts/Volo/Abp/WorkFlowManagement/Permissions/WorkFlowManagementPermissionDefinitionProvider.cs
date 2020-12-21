using Volo.Abp.WorkFlowManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Volo.Abp.WorkFlowManagement.Permissions
{
    public class WorkFlowManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(WorkFlowManagementPermissions.GroupName, L("Permission:WorkFlowManagement"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<WorkFlowManagementResource>(name);
        }
    }
}