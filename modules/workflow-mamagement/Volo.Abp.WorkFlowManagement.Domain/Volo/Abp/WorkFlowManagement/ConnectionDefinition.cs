using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.WorkFlowManagement
{
    public class ConnectionDefinition:Entity
    {
        public ConnectionDefinition()
        {
        
        }
        public ConnectionDefinition(string sourceActivityId, string destinationActivityId, string outcome)
        {
            SourceActivityId = sourceActivityId;
            DestinationActivityId = destinationActivityId;
            Outcome = outcome;
        }
        public WorkflowDefinitionVersion WorkflowDefinitionVersion { get; set; }
        public string SourceActivityId { get; set; }
        public string DestinationActivityId { get; set; }
        public string Outcome { get; set; }

        public override object[] GetKeys()
        {
            return new object[]{ SourceActivityId , DestinationActivityId };
        }
    }
}