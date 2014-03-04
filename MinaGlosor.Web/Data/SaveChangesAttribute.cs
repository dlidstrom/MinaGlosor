using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using MinaGlosor.Web.Controllers.Api;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Data
{
    public class SaveChangesAttribute : AsyncFilter
    {
        protected override async Task InternalActionExecuted(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var controller = actionExecutedContext.ActionContext.ControllerContext.Controller as ApiControllerBase;
            if (controller != null && actionExecutedContext.Exception == null)
                await controller.Context.SaveChangesAsync();
        }
    }
}