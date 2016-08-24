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
            // TODO: Check request headers
            var nvp = actionContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            if (nvp.ContainsKey("v") && nvp["v"] != Application.GetAppVersion())
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.UpgradeRequired,
                    "Det finns en nyare version, vänligen ladda om sidan");
            }

            base.OnActionExecuting(actionContext);
        }
    }
}