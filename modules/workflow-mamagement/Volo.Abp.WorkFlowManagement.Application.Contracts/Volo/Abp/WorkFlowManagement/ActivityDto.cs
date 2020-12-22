﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Volo.Abp.WorkFlowManagement
{
    public class ActivityDto
    {
        public ActivityDto()
        {
        }
        public string Id { get; set; }
        public string Type { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public Dictionary<string,object> State { get; set; }
        public bool Blocking { get; set; }
        public bool Executed { get; set; }
        public bool Faulted { get; set; }
        
        public ActivityMessageDto Message { get; set; }
    }
}