using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.WorkFlowManagement
{
    public interface IWorkflowInstanceRepository: IBasicRepository<WorkflowInstance, Guid>
    {
        Task<WorkflowInstance> GetByInstanceIdAsync(string instanceId,CancellationToken cancellationToken=default);
    }
}