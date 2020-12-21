using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.WorkFlowManagement
{
    public class ConnectionDefinition:Entity<Guid>
    {
        public WorkflowDefinitionVersion WorkflowDefinitionVersion { get; set; }
        public string SourceActivityId { get; set; }
        public string DestinationActivityId { get; set; }
        public string Outcome { get; set; }
    }
}