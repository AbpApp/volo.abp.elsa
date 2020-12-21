using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.WorkFlowManagement.EntityFrameworkCore
{
    public class EfCoreWorkflowInstanceRepository : EfCoreRepository<IWorkFlowManagementDbContext, WorkflowInstance, Guid>, IWorkflowInstanceRepository
    {
        public EfCoreWorkflowInstanceRepository(IDbContextProvider<IWorkFlowManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<WorkflowInstance> GetByCorrelationIdAsync(string lationId,  bool includeDetails = false,CancellationToken cancellationToken = default)
        {
            return await DbSet.IncludeDetails(includeDetails).FirstOrDefaultAsync(t => t.CorrelationId == lationId,cancellationToken);
        }

        public void BatchDeleteWorkflowInstances(List<WorkflowInstance> instances)
        {
            DbSet.RemoveRange(instances);
        }

        public void BatchDeleteBlockingActivitys(List<BlockingActivity> blockActivitys)
        {
            DbContext.Set<BlockingActivity>().RemoveRange(blockActivitys);
        }

        public async Task<List<WorkflowInstance>> GetListByDefinitionIdAsync(string definitionId, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.IncludeDetails(includeDetails).Where(t => t.DefinitionId == definitionId)
                .ToListAsync(cancellationToken);

        }

        public async Task<List<BlockingActivity>> GetBlockingActivityListByWorkFlowInstanceDefinitionIdAsync(string id,
            CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<BlockingActivity>()
                .Include(t=>t.WorkflowInstance)
                .Where(t => t.WorkflowInstance.InstanceId == id)
                .ToListAsync(cancellationToken);
        }

        public async Task<WorkflowInstance> GetByInstanceIdAsync(string instanceId,  bool includeDetails = false,CancellationToken cancellationToken = default)
        {
            return await DbSet.IncludeDetails(includeDetails).FirstOrDefaultAsync(t => t.InstanceId == instanceId,cancellationToken);
        }

        public async Task<List<WorkflowInstance>> GetListByBlockingActivityAsync(string activityType, string correlationId = null, WorkflowStatus status = WorkflowStatus.Executing,  bool includeDetails = false,CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(t => t.BlockingActivities.Any(t => t.ActivityType == activityType))
                .WhereIf(!string.IsNullOrWhiteSpace(correlationId), t => t.CorrelationId == correlationId)
                .OrderByDescending(t => t.CreationTime)
                .ToListAsync(cancellationToken);

        }
        public async Task<List<WorkflowInstance>> GetListByCorrelationIdAsync(string lationId, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await DbSet.IncludeDetails(includeDetails).Where(t => t.CorrelationId == lationId).ToListAsync(cancellationToken);
        }

        public async Task<List<WorkflowInstance>> GetListByStatusAsync(string definitionId = null, WorkflowStatus status = WorkflowStatus.Executing, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .WhereIf(!string.IsNullOrWhiteSpace(definitionId),t => t.DefinitionId == definitionId)
                .Where(t=>t.Status==status)
                .ToListAsync(cancellationToken);
        }
        
    }
}
