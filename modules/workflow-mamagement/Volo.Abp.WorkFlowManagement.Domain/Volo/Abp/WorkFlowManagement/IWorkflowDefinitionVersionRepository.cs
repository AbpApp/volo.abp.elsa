using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.WorkFlowManagement
{
    public interface IWorkflowDefinitionVersionRepository: IBasicRepository<WorkflowDefinitionVersion, string>
    {
        void BatchDeleteWorkflowDefinitionVersions(
            List<WorkflowDefinitionVersion> definitions
        );
        void BatchDeleteConnectionDefinitions(
            List<ConnectionDefinition> connectionDefinitions
        );
        Task<List<ConnectionDefinition>> GetConnectionDefinitionListByWorkFlowInstanceDefinitionIdAsync(
            string id,
            CancellationToken cancellationToken=default);
        Task<WorkflowDefinitionVersion> GetByVersionIdAsync(
            string versionId,
            bool includeDetails = false,
            CancellationToken cancellationToken=default);
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