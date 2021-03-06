﻿using Elsa.Models;
using System.Collections.Generic;

namespace Volo.Abp.WorkFlowManagement
{
    public class WorkflowDefinitionDto
    {
        public int Version { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Variables Variables { get; set; }
        public bool IsSingleton { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsPublished { get; set; } = false;
        public bool IsLatest { get; set; }

        public List<ActivityDto> Activities { get; set; }
        public List<ConnectionDto> Connections { get; set; }
    }
}