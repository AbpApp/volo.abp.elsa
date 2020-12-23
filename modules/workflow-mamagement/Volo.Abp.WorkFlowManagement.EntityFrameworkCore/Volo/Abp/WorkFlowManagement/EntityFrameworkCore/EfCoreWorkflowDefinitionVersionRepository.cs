using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.WorkFlowManagement.EntityFrameworkCore
{
    public class EfCoreWorkflowDefinitionVersionRepository:EfCoreRepository<IWorkFlowManagementDbContext, WorkflowDefinitionVersion, string>, IWorkflowDefinitionVersionRepository
    {
        public EfCoreWorkflowDefinitionVersionRepository(IDbContextProvider<IWorkFlowManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public void BatchDeleteWorkflowDefinitionVersions(List<WorkflowDefinitionVersion> definitions)
        {
            DbSet.RemoveRange(definitions);
        }

        public void BatchDeleteConnectionDefinitions(List<ConnectionDefinition> connectionDefinitions)
        {
            DbContext.Set<ConnectionDefinition>().RemoveRange(connectionDefinitions);
        }

        public Task<List<ConnectionDefinition>> GetConnectionDefinitionListByWorkFlowInstanceDefinitionIdAsync(string id,
            CancellationToken cancellationToken = default)
        {
            return DbContext.Set<ConnectionDefinition>()
                .Include(t=>t.WorkflowDefinitionVersion)
                .Where(t => t.WorkflowDefinitionVersion.Id == id)
                .ToListAsync(cancellationToken);
        }
        

        public async Task<WorkflowDefinitionVersion> GetByVersionIdAsync(string versionId, bool includeDetails = false,CancellationToken cancellationToken=default)
        {
            return await DbSet.IncludeDetails(includeDetails).FirstOrDefaultAsync(t => t.Id == versionId, cancellationToken);
        }

        public async Task<List<WorkflowDefinitionVersion>> GetListAsync(string sorting = null, int maxResultCount = 2147483647, int skipCount = 0, string filter = null,
            bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(filter) ||
                         x.Name.Contains(filter))
                .OrderBy(sorting ?? nameof(WorkflowDefinitionVersion.Name))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<WorkflowDefinitionVersion>> GetListByDefinitionIdAndVersionAsync(string id, VersionOptions version, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(t=>t.Id==id)
                .WithVersion(version)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<WorkflowDefinitionVersion> GetByIdAsync(string id, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await DbSet
             .IncludeDetails(includeDetails)
             .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }
    }
}