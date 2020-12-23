using System;
using Newtonsoft.Json.Linq;

namespace Volo.Abp.WorkFlowManagement
{
    public class ActivityInstanceListDto
    {
        public string ActivityId { get; set; }
        public string Type { get; set; }
        public JObject State { get; set; }
        public JObject Output { get; set; }

        public Guid HandlerUserId { get; set; }
    }
}