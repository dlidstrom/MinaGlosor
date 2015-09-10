using System;
using System.Diagnostics;
using System.Linq;
using System.Security;
using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models;
using Raven.Client;

namespace MinaGlosor.Web.Infrastructure
{
    public class CommandExecutor
    {
        private readonly IKernel kernel;

        public CommandExecutor(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public TResult ExecuteCommand<TResult>(ICommand<TResult> command, User user)
        {
            var handlerType = typeof(CommandHandlerBase<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var canExecuteMethod = handlerType.GetMethod("CanExecute");
            var handleMethod = handlerType.GetMethod("Handle");
            try
            {
                using (new ActivityScope(
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteCommandStart_3010,
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteCommandStop_3011,
                    command.GetType().Name))
                using (new ModelContext(Trace.CorrelationManager.ActivityId))
                {
                    var commandAsJson = command.ToJson();
                    object handler = null;
                    try
                    {
                        handler = kernel.Resolve(handlerType);

                        string userId = null;
                        string email = null;
                        if (user != null)
                        {
                            var canExecute = (bool)canExecuteMethod.Invoke(handler, new[] { command, (object)user });
                            if (canExecute == false)
                            {
                                var message = string.Format("Operation not allowed: {0} {1}", command.GetType().Name, user.Username);
                                throw new SecurityException(message);
                            }

                            userId = user.Id;
                            email = user.Email;
                        }

                        var documentSession = kernel.Resolve<IDocumentSession>();
                        var changeLogEntry = new ChangeLogEntry(
                            userId ?? "<unknown user>",
                            email ?? "<unknown email>",
                            Trace.CorrelationManager.ActivityId,
                            command.GetType(),
                            commandAsJson);

                        documentSession.Store(changeLogEntry);

                        var result = (TResult)handleMethod.Invoke(handler, new[] { (object)command });

                        var whatChanged = documentSession.Advanced.WhatChanged();
                        if (whatChanged.Any())
                        {
                            TracingLogger.Information("Saving {0} changes", whatChanged.Count);
                            documentSession.SaveChanges();
                        }

                        return result;
                    }
                    finally
                    {
                        if (handler != null) kernel.ReleaseComponent(handler);
                    }
                }
            }
            catch (ComponentNotFoundException ex)
            {
                var message = string.Format(
                    "No handler found for {0}. Implement CommandHandlerBase<{0}>",
                    command.GetType().Name);
                throw new ApplicationException(message, ex);
            }
        }
    }
}