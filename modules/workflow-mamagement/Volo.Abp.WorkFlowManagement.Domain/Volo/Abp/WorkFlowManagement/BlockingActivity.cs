using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.WorkFlowManagement
{
    public class BlockingActivity:Entity<string>
    {
        public BlockingActivity()
        {
            
        }
        public BlockingActivity(string id)
        {
            Id = id;
        }
        public WorkflowInstance WorkflowInstance { get; set; }
        public string ActivityId { get; set; }
        public string ActivityType { get; set; }
    }
}