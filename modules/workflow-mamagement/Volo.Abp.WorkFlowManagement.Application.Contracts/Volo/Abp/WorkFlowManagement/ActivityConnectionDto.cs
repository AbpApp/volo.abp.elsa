using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.WorkFlowManagement
{
    public class ActivityConnectionDto
    {
        public ActivityDto SourceActivity { get; set; }

        public ActivityDto DestinationActivity { get; set; }

        public string Outcome { get; set; }
    }
}
