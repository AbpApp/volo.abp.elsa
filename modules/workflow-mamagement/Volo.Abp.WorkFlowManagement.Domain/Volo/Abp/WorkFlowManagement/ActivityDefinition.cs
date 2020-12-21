using System;
using Newtonsoft.Json.Linq;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.WorkFlowManagement
{
    public class ActivityDefinition:FullAuditedAggregateRoot<Guid>
    {
        public string ActivityId { get; set; }
        public WorkflowDefinitionVersion WorkflowDefinitionVersion { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public JObject State { get; set; }
    }
}