using AutoMapper;
using Elsa.Metadata;

namespace Volo.Abp.WorkFlowManagement
{
    public class WorkFlowManagementApplicationAutoMapperProfile : Profile
    {
        public WorkFlowManagementApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<ActivityDescriptor, ActivityDescriptorDto>()
                .ForMember(t=>t.Id,option=>option.Ignore());
            CreateMap<WorkflowDefinitionVersion, WorkflowDefinitionDto>();
            CreateMap<ActivityDefinition, ActivityDto>();
            CreateMap<ConnectionDefinition, ConnectionDto>();
            CreateMap<ActivityInstance, ActivityInstanceDto>();
            CreateMap<ActivityInstance, ActivityInstanceListDto>();
            CreateMap<BlockingActivity, BlockingActivityDto>();
            CreateMap<BlockingActivity, BlockingActivityListDto>();
            CreateMap<WorkflowInstance, WorkflowInstanceDto>();
            CreateMap<WorkflowInstance, WorkflowInstanceListDto>();
            CreateMap<ConnectionDefinition, ConnectionDto>();
        }
    }
}