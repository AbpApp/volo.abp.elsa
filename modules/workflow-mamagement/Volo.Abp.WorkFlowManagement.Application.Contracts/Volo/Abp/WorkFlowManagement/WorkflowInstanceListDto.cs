using System;
using System.Collections.Generic;
using Elsa.Models;
using NodaTime;

namespace Volo.Abp.WorkFlowManagement
{
    public class WorkflowInstanceListDto
    {
        public string DefinitionId { get; set; }
        public int Version { get; set; }
        public WorkflowStatus Status { get; set; }
        public string CorrelationId { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? FinishedAt { get; set; }

        public DateTime? FaultedAt { get; set; }

        public DateTime? AbortedAt { get; set; }
        public WorkflowExecutionScope Scope { get; set; }
        public Variables Input { get; set; }
        public ICollection<LogEntry> ExecutionLog { get; set; }
    }
}