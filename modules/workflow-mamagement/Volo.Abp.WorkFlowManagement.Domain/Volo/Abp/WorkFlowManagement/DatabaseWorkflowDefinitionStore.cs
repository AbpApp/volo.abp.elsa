using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Persistence;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;


namespace Volo.Abp.WorkFlowManagement
{
    public class DatabaseWorkflowDefinitionStore: IWorkflowDefinitionStore
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IWorkflowDefinitionVersionRepository _workflowDefinitionVersionRepository;
        private readonly IObjectMapper<WorkFlowManagementDomainModule> _objectMapper;
        private readonly IActivityDefinitionRepository _activityDefinitionRepository;
        private readonly IActivityInstanceRepository _activityInstanceRepository;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;
        private readonly IGuidGenerator _guidGenerator;
        public DatabaseWorkflowDefinitionStore(IWorkflowDefinitionVersionRepository workflowDefinitionVersionRepository, IObjectMapper<WorkFlowManagementDomainModule> objectMapper, IActivityDefinitionRepository activityDefinitionRepository, IWorkflowInstanceRepository workflowInstanceRepository, IActivityInstanceRepository activityInstanceRepository, IUnitOfWorkManager unitOfWorkManager, IGuidGenerator guidGenerator)
        {
            _workflowDefinitionVersionRepository = workflowDefinitionVersionRepository;
            _objectMapper = objectMapper;
            _activityDefinitionRepository = activityDefinitionRepository;
            _workflowInstanceRepository = workflowInstanceRepository;
            _activityInstanceRepository = activityInstanceRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _guidGenerator = guidGenerator;
        }
        
        public async Task<global::Elsa.Models.WorkflowDefinitionVersion> SaveAsync(global::Elsa.Models.WorkflowDefinitionVersion definition, CancellationToken cancellationToken = default)
        {
            var existingEntity =
                await _workflowDefinitionVersionRepository
                    .GetByVersionIdAsync(definition.Id,true, cancellationToken);
            if (existingEntity == null)
                return await AddAsync(definition, cancellationToken);
            return await UpdateAsync(definition, cancellationToken);
        }
        [UnitOfWork]
        public  async  Task<global::Elsa.Models.WorkflowDefinitionVersion> AddAsync(global::Elsa.Models.WorkflowDefinitionVersion definition, CancellationToken cancellationToken = default)
        {
            var entity = Map(definition);
            await _workflowDefinitionVersionRepository.InsertAsync(entity, cancellationToken: cancellationToken);
            await _unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
            return Map(entity);
        }
        [UnitOfWork]
        public async Task<global::Elsa.Models.WorkflowDefinitionVersion> GetByIdAsync(string id, VersionOptions version, CancellationToken cancellationToken = default)
        {
            var lists = await _workflowDefinitionVersionRepository.GetListByDefinitionIdAndVersionAsync(id,version,false,cancellationToken);
            var entity = lists.FirstOrDefault();
            await _unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
            return Map(entity);
        }

        public  async Task<IEnumerable<global::Elsa.Models.WorkflowDefinitionVersion>> ListAsync(VersionOptions version, CancellationToken cancellationToken = default)
        {
            var lists = await _workflowDefinitionVersionRepository.GetListAsync(includeDetails:true);
            return _objectMapper.Map<IEnumerable<WorkflowDefinitionVersion>,IEnumerable<global::Elsa.Models.WorkflowDefinitionVersion>>(lists.AsEnumerable());
        }
        [UnitOfWork]
        public  async Task<global::Elsa.Models.WorkflowDefinitionVersion> UpdateAsync(global::Elsa.Models.WorkflowDefinitionVersion definition, CancellationToken cancellationToken = default)
        {
            var entity = await _workflowDefinitionVersionRepository
                .GetByVersionIdAsync(definition.Id,true,cancellationToken);
            entity = _objectMapper.Map(definition, entity);
            await _workflowDefinitionVersionRepository.UpdateAsync(entity, cancellationToken: cancellationToken);
            await _unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
            return Map(entity);
        }
        [UnitOfWork]
        public  async Task<int> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var definitionRecords =
                await _workflowDefinitionVersionRepository.GetListByDefinitionIdAndVersionAsync(id, VersionOptions.All,
                    true, cancellationToken);
            var instanceRecords = await _workflowInstanceRepository.GetListByDefinitionIdAsync(id,false,cancellationToken);
            var activityDefinitionRecords = await _activityDefinitionRepository
                .GetListByWorkFlowInstanceDefinitionIdAsync(id, cancellationToken: cancellationToken);
            var connectionRecords = await _workflowDefinitionVersionRepository
                .GetConnectionDefinitionListByWorkFlowInstanceDefinitionIdAsync(id, cancellationToken: cancellationToken);
            var activityInstanceRecords = await _activityInstanceRepository
                .GetListByWorkFlowInstanceDefinitionIdAsync(id, cancellationToken: cancellationToken);
            var blockingActivityRecords = await _workflowInstanceRepository
                .GetBlockingActivityListByWorkFlowInstanceDefinitionIdAsync(id, cancellationToken: cancellationToken);
            _workflowInstanceRepository.BatchDeleteWorkflowInstances(instanceRecords);
            _workflowDefinitionVersionRepository.BatchDeleteWorkflowDefinitionVersions(definitionRecords);
            _activityDefinitionRepository.BatchDeleteActivityDefinitions(activityDefinitionRecords);
            _workflowDefinitionVersionRepository.BatchDeleteConnectionDefinitions(connectionRecords);
            _activityInstanceRepository.BatchDeleteActivityInstances(activityInstanceRecords);
            _workflowInstanceRepository.BatchDeleteBlockingActivitys(blockingActivityRecords);
            await _unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
            return definitionRecords.Count;
        }
        private WorkflowDefinitionVersion Map(global::Elsa.Models.WorkflowDefinitionVersion source) => _objectMapper.Map<global::Elsa.Models.WorkflowDefinitionVersion,WorkflowDefinitionVersion>(source);
        private global::Elsa.Models.WorkflowDefinitionVersion Map(WorkflowDefinitionVersion source) => _objectMapper.Map<WorkflowDefinitionVersion,global::Elsa.Models.WorkflowDefinitionVersion>(source);

    }
}