using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.WorkFlowManagement
{
    public class ConnectionDefinition:Entity<string>
    {
        
        public ConnectionDefinition()
        {
        
        }
        public ConnectionDefinition(string id,string sourceActivityId, string destinationActivityId, string outcome)
        {
            Id = id;
            SourceActivityId = sourceActivityId;
            DestinationActivityId = destinationActivityId;
            Outcome = outcome;
        }
        public WorkflowDefinitionVersion WorkflowDefinitionVersion { get; set; }
        public string SourceActivityId { get; set; }
        public string DestinationActivityId { get; set; }
        public string Outcome { get; set; }
        
    }
}