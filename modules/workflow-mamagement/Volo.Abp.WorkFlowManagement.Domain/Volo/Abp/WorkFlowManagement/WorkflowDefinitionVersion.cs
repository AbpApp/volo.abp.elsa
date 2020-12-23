using System;
using System.Collections.Generic;
using Elsa.Models;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.WorkFlowManagement
{
    public class WorkflowDefinitionVersion:FullAuditedAggregateRoot<string>
    {
        public WorkflowDefinitionVersion()
        {
            Activities = new List<ActivityDefinition>();
            Connections = new List<ConnectionDefinition>();
            Variables = new Variables();
        }
        

        public WorkflowDefinitionVersion(string id)
        {
            Id = id;
            Activities = new List<ActivityDefinition>();
            Connections = new List<ConnectionDefinition>();
            Variables = new Variables();
        }
        public WorkflowDefinitionVersion(string id,string name,string description,bool isSingleton,bool isDisabled)
        {
            Id = id;
            Name = name;
            Description = description;
            IsSingleton = isSingleton;
            IsDisabled = isDisabled;
            Activities = new List<ActivityDefinition>();
            Connections = new List<ConnectionDefinition>();
            Variables = new Variables();
        }
        public int Version { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Variables Variables { get; set; }
        public bool IsSingleton { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsPublished { get; set; } = false;
        public bool IsLatest { get; set; }
        public ICollection<ActivityDefinition> Activities { get; set; }
        public ICollection<ConnectionDefinition> Connections { get; set; }
    }
}