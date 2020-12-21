using Elsa.Services;
using Volo.Abp.Collections;

namespace Volo.Abp.Elsa
{
    public class AbpElsaOptions
    {
        public ITypeList<IWorkflowProvider> DefinitionProviders { get; }

        public ActivityDefinitionList ActivityDefinitions { get; set; }

        public AbpElsaOptions()
        {
            DefinitionProviders= new TypeList<IWorkflowProvider>();
            ActivityDefinitions = new ActivityDefinitionList();
        }
    }
}
