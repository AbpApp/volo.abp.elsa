using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.WorkFlowManagement
{
    public class ActivityDefinition:Entity<string>
    {
        public ActivityDefinition()
        {
            State = new JObject();
        }
    
        public ActivityDefinition(string id, string type, Dictionary<string,object> state, int left = 0, int top = 0)
        {
            Id = id;
            Type = type;
            Left = left;
            State = JObject.Parse(JsonConvert.SerializeObject(state));
            Top = top;
        }
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