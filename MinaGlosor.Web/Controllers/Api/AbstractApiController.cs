using System;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Web.Http;
using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Attributes;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Indexes;
using Newtonsoft.Json;
using Raven.Client;

namespace MinaGlosor.Web.Controllers.Api
{
    [CheckAppVersion]
    public abstract class AbstractApiController : ApiController
    {
        private User currentUser;

        public IKernel Kernel { get; set; }

        public CommandExecutor CommandExecutor { get; set; }

        protected User CurrentUser
        {
            get
            {
                if (currentUser != null) return currentUser;

                currentUser = GetDocumentSession().Query<User, UserIndex>()
                                                  .FirstOrDefault(x => x.Email == User.Identity.Name);

                return currentUser;
            }
        }

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            var documentSession = GetDocumentSession();
            if (!query.CanExecute(documentSession, CurrentUser))
            {
                throw new SecurityException("Operation not allowed");
            }

            var result = query.Execute(documentSession);
            var whatChanged = documentSession.Advanced.WhatChanged();
            if (whatChanged.Any())
            {
                TracingLogger.Warning(
                    EventIds.Warning_Transient_4XXX.Web_ChangesFromQuery_4002,
                    "Change detected from query: {0}",
                    JsonConvert.SerializeObject(whatChanged, Formatting.Indented));
            }

            return result;
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            if (command == null) throw new ArgumentNullException("command");
            var result = CommandExecutor.ExecuteCommand(command, CurrentUser);
            return result;
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command, User runAs)
        {
            if (runAs == null) throw new ArgumentNullException("runAs");
            if (command == null) throw new ArgumentNullException("command");
            var result = CommandExecutor.ExecuteCommand(command, runAs);
            return result;
        }

        [DebuggerStepThrough]
        protected IDocumentSession GetDocumentSession()
        {
            return Kernel.Resolve<IDocumentSession>();
        }
    }
}