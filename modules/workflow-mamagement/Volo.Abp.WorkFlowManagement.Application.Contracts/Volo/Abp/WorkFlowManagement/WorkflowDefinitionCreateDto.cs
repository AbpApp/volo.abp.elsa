using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.WorkFlowManagement
{
    public class WorkflowDefinitionCreateDto
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSingleton { get; set; }
        public bool IsDisabled { get; set; }


        public List<ConnectionDto> Connections { get; set; }
        public List<ActivityDto> Activities { get; set; }
    }
}
