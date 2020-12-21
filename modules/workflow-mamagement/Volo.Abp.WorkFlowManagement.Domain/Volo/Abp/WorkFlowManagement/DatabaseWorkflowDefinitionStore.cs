using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Persistence;
using Volo.Abp.ObjectMapping;


namespace Volo.Abp.WorkFlowManagement
{
    public class DatabaseWorkflowDefinitionStore: IWorkflowDefinitionStore
    {
        private readonly IWorkflowDefinitionVersionRepository WorkflowDefinitionVersionRepository;
        private readonly IObjectMapper<WorkFlowManagementDomainModule> _objectMapper;
        public DatabaseWorkflowDefinitionStore(IWorkflowDefinitionVersionRepository workflowDefinitionVersionRepository, IObjectMapper<WorkFlowManagementDomainModule> objectMapper)
        {
            WorkflowDefinitionVersionRepository = workflowDefinitionVersionRepository;
            _objectMapper = objectMapper;
        }

        public async Task<global::Elsa.Models.WorkflowDefinitionVersion> SaveAsync(global::Elsa.Models.WorkflowDefinitionVersion definition, CancellationToken cancellationToken = default)
        {
            var existingEntity =
                await WorkflowDefinitionVersionRepository
                    .GetByVersionIdAsync(definition.Id, cancellationToken);
            if (existingEntity == null)
                return await AddAsync(definition, cancellationToken);
            return await UpdateAsync(definition, cancellationToken);
        }

        public  async  Task<global::Elsa.Models.WorkflowDefinitionVersion> AddAsync(global::Elsa.Models.WorkflowDefinitionVersion definition, CancellationToken cancellationToken = default)
        {
            var entity = Map(definition);
            await WorkflowDefinitionVersionRepository.InsertAsync(entity, true,cancellationToken);
            
            return Map(entity);
        }

        public async Task<global::Elsa.Models.WorkflowDefinitionVersion> GetByIdAsync(string id, VersionOptions version, CancellationToken cancellationToken = default)
        {
            var lists = await WorkflowDefinitionVersionRepository.GetListByDefinitionIdAndVersionAsync(id,version,false,cancellationToken);
            var entity = lists.FirstOrDefault();
            return Map(entity);
        }

        public  async Task<IEnumerable<global::Elsa.Models.WorkflowDefinitionVersion>> ListAsync(VersionOptions version, CancellationToken cancellationToken = default)
        {
            var lists = await WorkflowDefinitionVersionRepository.GetListAsync();
            return _objectMapper.Map<IEnumerable<WorkflowDefinitionVersion>,IEnumerable<global::Elsa.Models.WorkflowDefinitionVersion>>(lists.AsEnumerable());
        }

        public  async Task<global::Elsa.Models.WorkflowDefinitionVersion> UpdateAsync(global::Elsa.Models.WorkflowDefinitionVersion definition, CancellationToken cancellationToken = default)
        {
            var entity = await WorkflowDefinitionVersionRepository
                .GetByVersionIdAsync(definition.Id,cancellationToken);
            entity = _objectMapper.Map(definition, entity);
            await WorkflowDefinitionVersionRepository.UpdateAsync(entity);
            return Map(entity);
        }

        public Task<int> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
        private WorkflowDefinitionVersion Map(global::Elsa.Models.WorkflowDefinitionVersion source) => _objectMapper.Map<global::Elsa.Models.WorkflowDefinitionVersion,WorkflowDefinitionVersion>(source);
        private global::Elsa.Models.WorkflowDefinitionVersion Map(WorkflowDefinitionVersion source) => _objectMapper.Map<WorkflowDefinitionVersion,global::Elsa.Models.WorkflowDefinitionVersion>(source);

    }
}