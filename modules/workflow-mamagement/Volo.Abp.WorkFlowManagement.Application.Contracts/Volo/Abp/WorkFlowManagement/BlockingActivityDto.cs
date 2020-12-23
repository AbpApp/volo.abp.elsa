namespace Volo.Abp.WorkFlowManagement
{
    public class BlockingActivityDto
    {
        public WorkflowInstanceDto WorkflowInstance { get; set; }
        public string ActivityId { get; set; }
        public string ActivityType { get; set; }
    }
}