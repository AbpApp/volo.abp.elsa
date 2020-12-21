using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.WorkFlowManagement
{
    public interface IActivityDefinitionRepository:IBasicRepository<ActivityDefinition, Guid>
    {
        void BatchDeleteActivityDefinitions(
            List<ActivityDefinition> definitions
        );
        Task<List<ActivityDefinition>> GetListByWorkFlowInstanceDefinitionIdAsync(
            string id,
            CancellationToken cancellationToken=default);
    }
}