using System;
using Newtonsoft.Json.Linq;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.WorkFlowManagement
{
    public class ActivityInstance:FullAuditedAggregateRoot<Guid>
    {
        public Guid ActivityId { get; set; }
        public WorkflowInstance WorkflowInstance { get; set; }
        public string Type { get; set; }
        public JObject State { get; set; }
        public JObject Output { get; set; }
    }
}