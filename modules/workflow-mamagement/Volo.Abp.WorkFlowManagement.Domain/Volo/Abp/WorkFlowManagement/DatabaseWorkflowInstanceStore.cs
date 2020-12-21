using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Persistence;
using Volo.Abp.ObjectMapping;


namespace Volo.Abp.WorkFlowManagement
{
    public class DatabaseWorkflowInstanceStore: IWorkflowInstanceStore
    {
        private readonly IWorkflowInstanceRepository workflowInstanceRepository;
        private readonly IObjectMapper<WorkFlowManagementDomainModule> _objectMapper;

        public DatabaseWorkflowInstanceStore(IWorkflowInstanceRepository workflowInstanceRepository, IObjectMapper<WorkFlowManagementDomainModule> objectMapper)
        {
            this.workflowInstanceRepository = workflowInstanceRepository;
            _objectMapper = objectMapper;
        }

        public async Task<global::Elsa.Models.WorkflowInstance> SaveAsync(global::Elsa.Models.WorkflowInstance instance, CancellationToken cancellationToken = default)
        {
            var existingEntity = await workflowInstanceRepository
                .GetByInstanceIdAsync(instance.Id, cancellationToken );
            if (existingEntity == null)
            {
                var entity = Map(instance);


                return Map(entity);
            }
        }

        public Task<global::Elsa.Models.WorkflowInstance> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<global::Elsa.Models.WorkflowInstance> GetByCorrelationIdAsync(string correlationId, CancellationToken cancellationToken =default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<global::Elsa.Models.WorkflowInstance>> ListByDefinitionAsync(string definitionId, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<global::Elsa.Models.WorkflowInstance>> ListAllAsync(CancellationToken cancellationToken =default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<(global::Elsa.Models.WorkflowInstance, global::Elsa.Models.ActivityInstance)>> ListByBlockingActivityAsync(string activityType, string correlationId = null,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<global::Elsa.Models.WorkflowInstance>> ListByStatusAsync(string definitionId, global::Elsa.Models.WorkflowStatus status,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<global::Elsa.Models.WorkflowInstance>> ListByStatusAsync(global::Elsa.Models.WorkflowStatus status, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
        private WorkflowInstance Map(global::Elsa.Models.WorkflowInstance source) => _objectMapper.Map<global::Elsa.Models.WorkflowInstance,WorkflowInstance>(source);
        private global::Elsa.Models.WorkflowInstance Map(WorkflowInstance source) => _objectMapper.Map<WorkflowInstance,global::Elsa.Models.WorkflowInstance>(source);
        private IEnumerable<global::Elsa.Models.WorkflowInstance> Map(IEnumerable<WorkflowInstance> source) => _objectMapper.Map<IEnumerable<WorkflowInstance>,IEnumerable<global::Elsa.Models.WorkflowInstance>>(source);
    }
}