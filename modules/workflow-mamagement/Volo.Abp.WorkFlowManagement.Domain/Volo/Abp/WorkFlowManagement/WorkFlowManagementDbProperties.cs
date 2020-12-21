namespace Volo.Abp.WorkFlowManagement
{
    public static class WorkFlowManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "WorkFlowManagement";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "WorkFlowManagement";
    }
}
