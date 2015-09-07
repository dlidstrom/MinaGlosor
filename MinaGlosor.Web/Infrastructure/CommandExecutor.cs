using System;
using System.Diagnostics;
using System.Linq;
using System.Security;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models;
using Newtonsoft.Json;
using Raven.Client;

namespace MinaGlosor.Web.Infrastructure
{
    public class CommandExecutor
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new PrivateMembersContractResolver(),
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly IKernel kernel;

        public CommandExecutor(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public TResult ExecuteCommand<TResult>(User user, ICommand<TResult> command)
        {
            var handlerType = typeof(CommandHandlerBase<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var handleMethod = handlerType.GetMethod("Handle");
            try
            {
                using (kernel.BeginScope())
                using (new ActivityScope(
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteCommandStart_3010,
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteCommandStop_3011,
                    command.GetType().Name))
                using (new ModelContext(Trace.CorrelationManager.ActivityId))
                {
                    var commandAsJson = JsonConvert.SerializeObject(command, Formatting.Indented, Settings);
                    ICommandHandler handler = null;
                    try
                    {
                        handler = (ICommandHandler)kernel.Resolve(handlerType);
                        if (handler.CanExecute(user) == false)
                        {
                            throw new SecurityException("Operation not allowed");
                        }

                        var documentSession = kernel.Resolve<IDocumentSession>();
                        string userId = null;
                        string email = null;
                        if (user != null)
                        {
                            userId = user.Id;
                            email = user.Email;
                        }

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