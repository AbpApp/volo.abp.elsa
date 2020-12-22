using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.WorkFlowManagement.EntityFrameworkCore
{
    public class EfCoreActivityInstanceRepository:EfCoreRepository<IWorkFlowManagementDbContext, ActivityInstance, string>,IActivityInstanceRepository
    {
        public EfCoreActivityInstanceRepository(IDbContextProvider<IWorkFlowManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public void BatchDeleteActivityInstances(List<ActivityInstance> instances)
        {
           DbSet.RemoveRange(instances);
        }

        public async Task<List<ActivityInstance>> GetListByWorkFlowInstanceDefinitionIdAsync(string id, 
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(t=>t.WorkflowInstance)
                .Where(t => t.WorkflowInstance.DefinitionId == id)
                .ToListAsync(cancellationToken);
        }
    }
}