using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace MinaGlosor.Web.Infrastructure.Attributes
{
    public class CheckAppVersionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            IEnumerable<string> headerValues;
            if (!actionContext.Request.Headers.TryGetValues("Application-Version-Key", out headerValues)) return;

            var appVersion = Application.GetAppVersion();
            if (headerValues.Contains(appVersion) == false)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.UpgradeRequired,
                    "Det finns en nyare version, vänligen ladda om sidan");
            }

            base.OnActionExecuting(actionContext);
        }
    }
}