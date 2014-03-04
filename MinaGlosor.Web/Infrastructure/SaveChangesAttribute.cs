using System.Web.Http.Filters;
using MinaGlosor.Web.Controllers.Api;

namespace MinaGlosor.Web.Infrastructure
{
    public class SaveChangesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var controller = actionExecutedContext.ActionContext.ControllerContext.Controller as ApiControllerBase;
            if (controller == null || actionExecutedContext.Exception != null)
                return;

            controller.SaveChanges();
        }
    }
}