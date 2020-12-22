using System;
using Elsa.Metadata;

namespace Volo.Abp.WorkFlowManagement
{
    public class ActivityDescriptorDto
    {
        public ActivityDescriptorDto()
        {
            this.Type = "Activity";
            this.Properties = new ActivityPropertyDescriptor[0];
            this.Category = "Miscellaneous";
            this.DisplayName = "Activity";
            this.Properties = new ActivityPropertyDescriptor[0];
            this.Outcomes = (object) null;
        }

        public Guid Id { get; set; }
        public string Type { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string RuntimeDescription { get; set; }

        public string Category { get; set; }

        public string Icon { get; set; }

        public object Outcomes { get; set; }

        public ActivityPropertyDescriptor[] Properties { get; set; }
    }
}