﻿using Elsa.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Elsa;

namespace Volo.Abp.WorkFlowManagement
{
    public class ActivityDefinitionAppService : WorkFlowManagementAppService, IActivityDefinitionAppService
    {
        private readonly IActivityManager _activityManager;

        public ActivityDefinitionAppService(IActivityManager activityManager)
        {
            _activityManager = activityManager;
        }
        public List<ActivityDescriptorDto> GetActivityDescriptors()
        {
           return _activityManager.GetActivityDefinitionList().Select(item =>
            {
                var dto = ObjectMapper.Map<ActivityDescriptor, ActivityDescriptorDto>(item);
                dto.Id = GuidGenerator.Create();
                return dto;
            }).ToList();
        }
    }
}
