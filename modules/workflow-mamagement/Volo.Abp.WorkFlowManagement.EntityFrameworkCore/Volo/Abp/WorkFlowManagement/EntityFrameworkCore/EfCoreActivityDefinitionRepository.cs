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
    public class EfCoreActivityDefinitionRepository:EfCoreRepository<IWorkFlowManagementDbContext, ActivityDefinition, string>,IActivityDefinitionRepository
    {
        public EfCoreActivityDefinitionRepository(IDbContextProvider<IWorkFlowManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public void BatchDeleteActivityDefinitions(List<ActivityDefinition> definitions)
        {
            DbSet.RemoveRange(definitions);
        }

        public Task<List<ActivityDefinition>> GetListByWorkFlowInstanceDefinitionIdAsync(string id, 
            CancellationToken cancellationToken = default)
        {
            return DbSet
                .Include(t=>t.WorkflowDefinitionVersion)
                .Where(t => t.WorkflowDefinitionVersion.Id == id)
                .ToListAsync(cancellationToken);
        }
    }
}