using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MinaGlosor.Web.Infrastructure.Attributes
{
    public class CorrelationIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Trace.CorrelationManager.ActivityId = SystemGuid.NewSequential;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Trace.CorrelationManager.ActivityId = default(Guid);
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            Trace.CorrelationManager.ActivityId = SystemGuid.NewSequential;
            return Task.FromResult(0);
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            Trace.CorrelationManager.ActivityId = default(Guid);
            return Task.FromResult(0);
        }
    }
}