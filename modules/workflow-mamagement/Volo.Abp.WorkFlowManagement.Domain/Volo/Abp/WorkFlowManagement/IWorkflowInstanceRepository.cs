using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.WorkFlowManagement
{
    public interface IWorkflowInstanceRepository: IBasicRepository<WorkflowInstance, Guid>
    {
        void BatchDeleteWorkflowInstances(
            List<WorkflowInstance> instances);
        void BatchDeleteBlockingActivitys(
            List<BlockingActivity> blockActivitys);
        Task<List<WorkflowInstance>> GetListByDefinitionIdAsync(
            string definitionId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);
        Task<List<BlockingActivity>> GetBlockingActivityListByWorkFlowInstanceDefinitionIdAsync(
            string id,
            CancellationToken cancellationToken=default);
        Task<WorkflowInstance> GetByInstanceIdAsync(string instanceId, bool includeDetails = false, CancellationToken cancellationToken=default);
        Task<WorkflowInstance> GetByCorrelationIdAsync(string lationId, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<List<WorkflowInstance>> GetListByCorrelationIdAsync(
            string lationId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);
       
        Task<List<WorkflowInstance>> GetListByBlockingActivityAsync(string activityType,string correlationId=null, WorkflowStatus status= WorkflowStatus.Executing, bool includeDetails = false,CancellationToken cancellationToken = default);
        Task<List<WorkflowInstance>> GetListByStatusAsync(
            string definitionId=null,
            WorkflowStatus status= global::Elsa.Models.WorkflowStatus.Executing, 
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

    }
}