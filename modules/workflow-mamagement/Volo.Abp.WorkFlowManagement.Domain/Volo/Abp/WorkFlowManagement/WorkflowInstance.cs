using System;
using System.Collections.Generic;
using System.Text;
using Elsa.Models;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.WorkFlowManagement
{
    public class WorkflowInstance:FullAuditedAggregateRoot<Guid>
    {

        public Guid InstanceId { get; set; }
        public string DefinitionId { get; set; }
        public int Version { get; set; }
        public WorkflowStatus Status { get; set; }
        public string CorrelationId { get; set; }
        public DateTime? StartedTime { get; set; }
        public DateTime? FinishedTime { get; set; }
        public DateTime? FaultedAtTime { get; set; }
        public DateTime? AbortedAtTime { get; set; }
        public WorkflowExecutionScope Scope { get; set; }
        public Variables Input { get; set; }
        public ICollection<LogEntry> ExecutionLog { get; set; }
        public WorkflowFault Fault { get; set; }
        public ICollection<ActivityInstance> Activities { get; set; }
        public ICollection<BlockingActivity> BlockingActivities { get; set; }
    }
}
