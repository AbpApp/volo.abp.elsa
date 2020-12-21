using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Elsa
{
    public class ActivityManager : IActivityManager
    {
        private readonly AbpElsaOptions Options;
        public ActivityManager(IOptions<AbpElsaOptions> options)
        {
            Options = options.Value;
        }
        public ActivityDefinitionList GetActivityDefinitionList()
        {
            return Options.ActivityDefinitions;
        }
    }
}
