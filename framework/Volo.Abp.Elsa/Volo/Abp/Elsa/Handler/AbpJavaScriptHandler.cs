using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;

namespace Volo.Abp.Elsa.Handler
{
    public class AbpJavaScriptHandler : INotificationHandler<EvaluatingJavaScriptExpression>
    {
        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var engine = notification.Engine;
            var context = notification.WorkflowExecutionContext;
            engine.SetValue("getSetting", (Func<string, object>) (message => $"Hello {message}!"));
            engine.SetValue("setV", (Func<string, object>) (message => $"Hello {message}!"));

            return Task.CompletedTask;

        }
    }
}