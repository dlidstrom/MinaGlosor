using System;
using System.Web.Http.Filters;

namespace MinaGlosor.Test.Api.Infrastructure
{
    public class WaitForIndexingFilter : ActionFilterAttribute
    {
        private readonly Action action;

        public WaitForIndexingFilter(Action action)
        {
            this.action = action;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            action.Invoke();
        }
    }
}