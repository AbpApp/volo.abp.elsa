using Volo.Abp.EventBus;

namespace Volo.Abp.Elsa
{
    [EventName("WorkFlow.Instance.Resume")]
    public class WorkflowInstanceResumeEto
    {
        public string WorkflowInstanceId { get; set; }
    }
}