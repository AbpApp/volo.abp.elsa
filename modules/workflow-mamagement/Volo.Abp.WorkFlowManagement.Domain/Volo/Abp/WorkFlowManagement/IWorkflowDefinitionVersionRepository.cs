using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.WorkFlowManagement
{
    public interface IWorkflowDefinitionVersionRepository: IBasicRepository<WorkflowDefinitionVersion, Guid>
    {
        Task<WorkflowDefinitionVersion> GetByVersionIdAsync(string versionId,CancellationToken cancellationToken=default);
        Task<List<WorkflowDefinitionVersion>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );
        Task<List<WorkflowDefinitionVersion>> GetListByDefinitionIdAndVersionAsync(
            string id ,
            VersionOptions version,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );
    }
}