using Elsa.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.WorkFlowManagement
{
    public interface IActivityDefinitionAppService: IApplicationService
    {
        ActivityDescriptor[] GetActivityDescriptors();
    }
}
