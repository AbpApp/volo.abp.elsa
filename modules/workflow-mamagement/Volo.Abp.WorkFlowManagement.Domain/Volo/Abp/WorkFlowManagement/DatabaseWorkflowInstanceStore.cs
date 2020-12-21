using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Extensions;
using Elsa.Models;
using Elsa.Persistence;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;


namespace Volo.Abp.WorkFlowManagement
{
    public class DatabaseWorkflowInstanceStore : IWorkflowInstanceStore
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;
        private readonly IObjectMapper<WorkFlowManagementDomainModule> _objectMapper;
        private readonly IActivityDefinitionRepository _activityDefinitionRepository;
        private readonly IActivityInstanceRepository _activityInstanceRepository;
        public DatabaseWorkflowInstanceStore(IWorkflowInstanceRepository workflowInstanceRepository, IObjectMapper<WorkFlowManagementDomainModule> objectMapper, IActivityDefinitionRepository activityDefinitionRepository, IActivityInstanceRepository activityInstanceRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            this._workflowInstanceRepository = workflowInstanceRepository;
            _objectMapper = objectMapper;
            _activityDefinitionRepository = activityDefinitionRepository;
            _activityInstanceRepository = activityInstanceRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<global::Elsa.Models.WorkflowInstance> SaveAsync(global::Elsa.Models.WorkflowInstance instance, CancellationToken cancellationToken = default)
        {
            var existingEntity = await _workflowInstanceRepository
                .GetByInstanceIdAsync(instance.Id, true,cancellationToken);
            if (existingEntity == null)
            {
                var entity = Map(instance);
                await _workflowInstanceRepository.InsertAsync(entity, true, cancellationToken);
                await _unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);

                return Map(entity);
            }
            else
            {
                _activityInstanceRepository.BatchDeleteActivityInstances(existingEntity.Activities.ToList());
                _workflowInstanceRepository.BatchDeleteBlockingActivitys(existingEntity.BlockingActivities.ToList());
                existingEntity.Activities.Clear();
                existingEntity.BlockingActivities.Clear();
                var entity = _objectMapper.Map(instance, existingEntity);
                await _workflowInstanceRepository.UpdateAsync(entity, cancellationToken: cancellationToken);
                await _unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);

                return Map(entity);
            }
        }

        public async Task<global::Elsa.Models.WorkflowInstance> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var existingEntity = await _workflowInstanceRepository
               .GetByInstanceIdAsync(id,true, cancellationToken);
            
            return Map(existingEntity);
        }

        public async Task<global::Elsa.Models.WorkflowInstance> GetByCorrelationIdAsync(string correlationId, CancellationToken cancellationToken = default)
        {
            var document = await _workflowInstanceRepository
                .GetByCorrelationIdAsync(correlationId,true, cancellationToken);

            return Map(document);
        }

        public async Task<IEnumerable<global::Elsa.Models.WorkflowInstance>> ListByDefinitionAsync(string definitionId, CancellationToken cancellationToken = default)
        {
            var documents = await _workflowInstanceRepository
                .GetListByCorrelationIdAsync(definitionId, true,cancellationToken);

            return Map(documents);
        }

        public async Task<IEnumerable<global::Elsa.Models.WorkflowInstance>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            var documents = await _workflowInstanceRepository
              .GetListAsync();
            return Map(documents);
        }

        public async Task<IEnumerable<(global::Elsa.Models.WorkflowInstance, global::Elsa.Models.ActivityInstance)>> ListByBlockingActivityAsync(string activityType, string correlationId = null,
            CancellationToken cancellationToken = default)
        {
            var documents = await _workflowInstanceRepository
            .GetListByBlockingActivityAsync(activityType, correlationId, WorkflowStatus.Executing,true, cancellationToken);
            var instances = Map(documents);

            return instances.GetBlockingActivities(activityType);
        }

        public async Task<IEnumerable<global::Elsa.Models.WorkflowInstance>> ListByStatusAsync(string definitionId, global::Elsa.Models.WorkflowStatus status,
            CancellationToken cancellationToken = default)
        {
            var documents = await _workflowInstanceRepository
           .GetListByStatusAsync(definitionId, status,true, cancellationToken);
            return Map(documents);
        }

        public async Task<IEnumerable<global::Elsa.Models.WorkflowInstance>> ListByStatusAsync(global::Elsa.Models.WorkflowStatus status, CancellationToken cancellationToken = default)
        {
            var documents = await _workflowInstanceRepository
           .GetListByStatusAsync(null, status,true, cancellationToken);
            return Map(documents);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var record = await _workflowInstanceRepository.GetByInstanceIdAsync(id, cancellationToken: cancellationToken);
            if (record == null)
                return;
            var activityInstanceRecords = await _activityDefinitionRepository
                .GetListByWorkFlowInstanceDefinitionIdAsync(id, cancellationToken);
            var blockingActivityRecords = await _workflowInstanceRepository
                .GetBlockingActivityListByWorkFlowInstanceDefinitionIdAsync(id, cancellationToken);
            _activityDefinitionRepository.BatchDeleteActivityDefinitions(activityInstanceRecords);
            _workflowInstanceRepository.BatchDeleteBlockingActivitys(blockingActivityRecords);
            await  _workflowInstanceRepository.DeleteAsync(record, cancellationToken: cancellationToken);
            await _unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);

        }
        private WorkflowInstance Map(global::Elsa.Models.WorkflowInstance source) => _objectMapper.Map<global::Elsa.Models.WorkflowInstance, WorkflowInstance>(source);
        private global::Elsa.Models.WorkflowInstance Map(WorkflowInstance source) => _objectMapper.Map<WorkflowInstance, global::Elsa.Models.WorkflowInstance>(source);
        private IEnumerable<global::Elsa.Models.WorkflowInstance> Map(IEnumerable<WorkflowInstance> source) => _objectMapper.Map<IEnumerable<WorkflowInstance>, IEnumerable<global::Elsa.Models.WorkflowInstance>>(source);
    }
}