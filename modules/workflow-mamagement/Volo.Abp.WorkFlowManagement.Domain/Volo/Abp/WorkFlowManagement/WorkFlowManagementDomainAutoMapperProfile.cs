using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Volo.Abp.WorkFlowManagement
{
   public class WorkFlowManagementDomainAutoMapperProfile : Profile
    {
        public WorkFlowManagementDomainAutoMapperProfile()
        {
            CreateMap<global::Elsa.Models.WorkflowDefinitionVersion, WorkflowDefinitionVersion>();
            
            CreateMap<WorkflowDefinitionVersion, global::Elsa.Models.WorkflowDefinitionVersion>();

            CreateMap<global::Elsa.Models.WorkflowInstance, WorkflowInstance>()
                .ForMember(d => d.Id, d => d.Ignore())
                .ForMember(d => d.Activities, d => d.ConvertUsing(new ActivityInstanceDictionaryConverter()))
                .ForMember(d => d.InstanceId, d => d.MapFrom(s => s.Id));

            CreateMap<WorkflowInstance, global::Elsa.Models.WorkflowInstance>()
                .ForMember(d => d.Activities, d => d.ConvertUsing(new ActivityInstanceEntityCollectionConverter()))
                .ForMember(d => d.Id, d => d.MapFrom(s => s.InstanceId));

            CreateMap<global::Elsa.Models.ActivityDefinition, ActivityDefinition>()
                .ForMember(d => d.WorkflowDefinitionVersion, d => d.Ignore());

            CreateMap<ActivityDefinition, global::Elsa.Models.ActivityDefinition>();

            CreateMap<global::Elsa.Models.ActivityInstance, ActivityInstance>()
                .ForMember(d => d.Id, d => d.Ignore())
                .ForMember(d => d.ActivityId, d => d.MapFrom(s => s.Id))
                .ForMember(d => d.WorkflowInstance, d => d.Ignore());

            CreateMap<ActivityInstance, global::Elsa.Models.ActivityInstance>().ForMember(d => d.Id, d => d.MapFrom(s => s.ActivityId));
            CreateMap<global::Elsa.Models.BlockingActivity, BlockingActivity>()
                .ForMember(d => d.WorkflowInstance, d => d.Ignore())
                .ReverseMap();
            CreateMap<global::Elsa.Models.ConnectionDefinition, ConnectionDefinition>()
                .ForMember(d => d.WorkflowDefinitionVersion, d => d.Ignore())
                .ReverseMap();
        }
    }
    public class ActivityInstanceEntityCollectionConverter : IValueConverter<ICollection<ActivityInstance>, IDictionary<string, global::Elsa.Models.ActivityInstance>>
    {
        public IDictionary<string, global::Elsa.Models.ActivityInstance> Convert(ICollection<ActivityInstance> sourceMember, ResolutionContext context)
        {
            return sourceMember.ToDictionary(x => x.ActivityId, x => context.Mapper.Map<global::Elsa.Models.ActivityInstance>(x));
        }
    }

    public class ActivityInstanceDictionaryConverter : IValueConverter<IDictionary<string, global::Elsa.Models.ActivityInstance>, ICollection<ActivityInstance>>
    {
        public ICollection<ActivityInstance> Convert(IDictionary<string, global::Elsa.Models.ActivityInstance> sourceMember, ResolutionContext context) =>
            sourceMember.Select(x => context.Mapper.Map<ActivityInstance>(x.Value)).ToList();
    }
}
