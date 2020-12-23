using System.Threading;
using System.Threading.Tasks;
using Elsa.Scripting.Liquid.Helpers;
using Elsa.Scripting.Liquid.Messages;
using Elsa.Services.Models;
using Fluid;
using MediatR;
using Volo.Abp.Identity;

namespace Volo.Abp.WorkFlowManagement
{
    public class AbpLiquidContextHandler: INotificationHandler<EvaluatingLiquidExpression>
    {
        private readonly IdentityUserManager _identityUserManager;
        private readonly IdentityRoleManager _identityRoleManager;

        public AbpLiquidContextHandler(IdentityUserManager identityUserManager, IdentityRoleManager identityRoleManager)
        {
            _identityUserManager = identityUserManager;
            _identityRoleManager = identityRoleManager;
        }

        public Task Handle(EvaluatingLiquidExpression notification, CancellationToken cancellationToken)
        {
            var context = notification.TemplateContext;
            
            context.MemberAccessStrategy.Register<WorkflowExecutionContext, LiquidObjectAccessor<IdentityUser>>("Users", x => new LiquidObjectAccessor<IdentityUser>(name => GetUserAsync(x, name)));
            context.MemberAccessStrategy.Register<WorkflowExecutionContext, LiquidObjectAccessor<IdentityRole>>("Roles", x => new LiquidObjectAccessor<IdentityRole>(name => GetRoleAsync(x, name)));

            return Task.CompletedTask;
        }
        
        private  async  Task<IdentityUser> GetUserAsync(WorkflowExecutionContext executionContext, string name) 
            => await _identityUserManager.FindByNameAsync(name);
        private  async  Task<IdentityRole> GetRoleAsync(WorkflowExecutionContext executionContext, string name) 
            => await _identityRoleManager.FindByNameAsync(name);
    }
}