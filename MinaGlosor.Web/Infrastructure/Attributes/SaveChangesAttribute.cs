using System.Linq;
using System.Web.Http.Filters;
using System.Web.Mvc;
using MinaGlosor.Web.Controllers.Api;
using MinaGlosor.Web.Infrastructure.Tracing;
using Raven.Client;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace MinaGlosor.Web.Infrastructure.Attributes
{
    public class SaveChangesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var controller = actionExecutedContext.ActionContext.ControllerContext.Controller as AbstractApiController;
            if (controller == null || actionExecutedContext.Exception != null)
                return;

            var documentSession = DependencyResolver.Current.GetService<IDocumentSession>();
            var whatChanged = documentSession.Advanced.WhatChanged();
            if (whatChanged.Any())
            {
                TracingLogger.Information("Saving {0} changes", whatChanged.Count);
                documentSession.SaveChanges();
            }
        }
    }
}