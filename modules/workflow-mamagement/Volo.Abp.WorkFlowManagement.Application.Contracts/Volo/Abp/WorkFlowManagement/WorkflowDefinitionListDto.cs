using ElsaWorkflowDefinitionVersion =Elsa.Models.WorkflowDefinitionVersion;
namespace Volo.Abp.WorkFlowManagement
{
    public class WorkflowDefinitionListDto
    {
        public ElsaWorkflowDefinitionVersion WorkflowDefinition { get; set; }
        public int ExecutingCount { get; set; }
        public int FaultedCount { get; set; }
        public int AbortedCount { get; set; }
        public int FinishedCount { get; set; }
    }
}