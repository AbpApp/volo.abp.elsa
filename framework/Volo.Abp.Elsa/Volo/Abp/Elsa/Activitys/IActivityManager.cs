using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Elsa
{
    public interface IActivityManager
    {
        
        ActivityDefinitionList GetActivityDefinitionList();
    }
}
