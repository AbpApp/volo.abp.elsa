using Volo.Abp.Reflection;

namespace Volo.Abp.WorkFlowManagement.Permissions
{
    public class WorkFlowManagementPermissions
    {
        public const string GroupName = "WorkFlowManagement";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(WorkFlowManagementPermissions));
        }
    }
}