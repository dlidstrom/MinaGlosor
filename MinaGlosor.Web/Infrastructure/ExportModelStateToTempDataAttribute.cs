using System;
using System.Web.Mvc;

namespace MinaGlosor.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExportModelStateToTempDataAttribute : ModelStateTempDataTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Only copy when ModelState is invalid and we're performing a Redirect (i.e. PRG)
            if (!filterContext.Controller.ViewData.ModelState.IsValid &&
                (filterContext.Result is RedirectResult || filterContext.Result is RedirectToRouteResult))
            {
                ExportModelStateToTempData(filterContext);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}