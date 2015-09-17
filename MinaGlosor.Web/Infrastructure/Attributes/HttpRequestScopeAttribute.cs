using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MinaGlosor.Web.Infrastructure.Tracing;

namespace MinaGlosor.Web.Infrastructure.Attributes
{
    public class HttpRequestScopeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            TracingLogger.Start(EventIds.Informational_Preliminary_1XXX.Web_Request_Executing_1001);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                TracingLogger.Error(EventIds.Error_Permanent_5XXX.Web_UnhandledException_5000, actionExecutedContext.Exception);
            }

            TracingLogger.Stop(EventIds.Informational_Completion_2XXX.Web_Request_Executed_2001);
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            TracingLogger.Start(EventIds.Informational_Preliminary_1XXX.Web_Request_Executing_1001);
            return Task.FromResult(0);
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Exception != null)
            {
                TracingLogger.Error(EventIds.Error_Permanent_5XXX.Web_UnhandledException_5000, actionExecutedContext.Exception);
            }

            TracingLogger.Stop(EventIds.Informational_Completion_2XXX.Web_Request_Executed_2001);
            return Task.FromResult(0);
        }
    }
}