﻿using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.WorkFlowManagement
{
    public class BlockingActivity:Entity<Guid>
    {
        public WorkflowInstance WorkflowInstance { get; set; }
        public Guid ActivityId { get; set; }
        public string ActivityType { get; set; }
    }
}