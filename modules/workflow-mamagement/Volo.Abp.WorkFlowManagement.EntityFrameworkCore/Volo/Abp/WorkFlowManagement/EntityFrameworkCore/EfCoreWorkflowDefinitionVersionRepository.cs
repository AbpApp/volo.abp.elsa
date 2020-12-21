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
    public class EfCoreWorkflowDefinitionVersionRepository:EfCoreRepository<IWorkFlowManagementDbContext, WorkflowDefinitionVersion, Guid>, IWorkflowDefinitionVersionRepository
    {
        public EfCoreWorkflowDefinitionVersionRepository(IDbContextProvider<IWorkFlowManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<WorkflowDefinitionVersion> GetByVersionIdAsync(string versionId, CancellationToken cancellationToken=default)
        {
            return await DbSet.FirstOrDefaultAsync(t => t.VersionId == versionId, cancellationToken);
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
                .Where(t=>t.VersionId==id)
                .WithVersion(version)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}