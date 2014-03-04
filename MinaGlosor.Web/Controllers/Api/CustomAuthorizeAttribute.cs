using System.Web.Http;
using System.Web.Http.Controllers;

namespace MinaGlosor.Web.Controllers.Api
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public static bool DisableAuthorize { get; set; }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return DisableAuthorize || base.IsAuthorized(actionContext);
        }
    }
}