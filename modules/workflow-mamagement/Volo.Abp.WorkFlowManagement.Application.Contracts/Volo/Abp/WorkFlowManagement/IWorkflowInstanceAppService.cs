using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.WorkFlowManagement
{
    public interface IWorkflowInstanceAppService : IApplicationService
    {
        Task CreateByDefinitionIdAsync(string id);
        
        Task DeleteAsync(string id);
        
        Task<WorkflowInstanceDto> GetAsync(string id);
        Task<List<WorkflowInstanceListDto>> GetListAsync();
    }
}
