﻿using System;
using System.Collections.Generic;
using System.Text;
using Elsa.Models;
using NodaTime;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.WorkFlowManagement
{
    public class WorkflowInstance:FullAuditedAggregateRoot<string>
    {

        public WorkflowInstance()
        {
            
        }
        public WorkflowInstance(string id)
        {
            Id = id;
        }
        
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
        public WorkflowFault Fault { get; set; }
        public ICollection<ActivityInstance> Activities { get; set; }
        public ICollection<BlockingActivity> BlockingActivities { get; set; }
    }
}
