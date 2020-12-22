using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.WorkFlowManagement
{
    public interface IActivityInstanceRepository:IBasicRepository<ActivityInstance, string>
    {
        void BatchDeleteActivityInstances(
            List<ActivityInstance> instances
        );
        Task<List<ActivityInstance>> GetListByWorkFlowInstanceDefinitionIdAsync(
            string id,
            CancellationToken cancellationToken=default);
    }
}