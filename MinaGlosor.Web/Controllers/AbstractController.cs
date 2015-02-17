using System;
using System.Linq;
using System.Web.Mvc;
using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Controllers
{
    public class AbstractController : Controller
    {
        public IKernel Kernel { get; set; }

        protected User CurrentUser { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Response.IsRequestBeingRedirected) return;

            var documentSession = GetDocumentSession();
            var user = documentSession.Query<User, UserIndex>().SingleOrDefault(x => x.Email == User.Identity.Name);
            if (user != null)
                CurrentUser = user;

            // make sure there's an admin user
            var firstAdminUser = documentSession.Query<User, UserIndex>()
                                                .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
                                                .FirstOrDefault(x => x.Role == UserRole.Admin);
            if (firstAdminUser != null)
                return;

            // first launch
            Response.Redirect("~/welcome");
            Response.End();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction || filterContext.Exception != null) return;

            var documentSession = GetDocumentSession();
            if (documentSession.Advanced.WhatChanged().Any())
            {
                documentSession.SaveChanges();
            }
        }

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.Execute(GetDocumentSession());
        }

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            command.Execute(GetDocumentSession());
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            if (command == null) throw new ArgumentNullException("command");
            return command.Execute(GetDocumentSession());
        }

        private IDocumentSession GetDocumentSession()
        {
            return Kernel.Resolve<IDocumentSession>();
        }
    }
}