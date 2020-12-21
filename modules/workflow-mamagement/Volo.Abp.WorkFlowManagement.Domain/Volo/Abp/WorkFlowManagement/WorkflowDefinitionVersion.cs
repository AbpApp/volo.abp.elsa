using System;
using System.Collections.Generic;
using Elsa.Models;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.WorkFlowManagement
{
    public class WorkflowDefinitionVersion:FullAuditedAggregateRoot<Guid>
    {
        public string VersionId { get; set; }
        public string DefinitionId { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Variables Variables { get; set; }
        public bool IsSingleton { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsPublished { get; set; }
        public bool IsLatest { get; set; }
        public ICollection<ActivityDefinition> Activities { get; set; }
        public ICollection<ConnectionDefinition> Connections { get; set; }
    }
}