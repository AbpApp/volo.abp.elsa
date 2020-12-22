using System.Threading;
using System.Threading.Tasks;
using Elsa.Scripting.Liquid.Helpers;
using Elsa.Scripting.Liquid.Messages;
using Elsa.Services.Models;
using Fluid;
using MediatR;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace Volo.Abp.Elsa
{
    public class AbpLiquidContextHandler : INotificationHandler<EvaluatingLiquidExpression>
    {
        private readonly ICurrentUser CurrentUser;
        private readonly ICurrentTenant CurrentTenant;

        public AbpLiquidContextHandler(ICurrentUser currentUser, ICurrentTenant currentTenant)
        {
            CurrentUser = currentUser;
            CurrentTenant = currentTenant;
        }
        public Task Handle(EvaluatingLiquidExpression notification, CancellationToken cancellationToken)
        {
            var context = notification.TemplateContext;

           // context.MemberAccessStrategy.Register<ICurrentUser>("CurrentUser", x => new LiquidObjectAccessor<ICurrentUser>(name => GetCurrentUserAsync(name)));
           // context.MemberAccessStrategy.Register<ICurrentUser>("CurrentTenant", x => CurrentTenant);

            return Task.CompletedTask;
        }

        private Task<ICurrentUser> GetCurrentUserAsync(string name)
     => Task.FromResult(CurrentUser);
        private Task<ICurrentTenant> GetCurrentTenantAsync()
 => Task.FromResult(CurrentTenant);
    }
}
