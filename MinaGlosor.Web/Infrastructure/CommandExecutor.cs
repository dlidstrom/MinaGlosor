using System;
using System.Diagnostics;
using System.Linq;
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
        private readonly IKernel kernel;

        private readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new PrivateMembersContractResolver(),
            TypeNameHandling = TypeNameHandling.All
        };

        public CommandExecutor(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public void ExecuteCommand<TCommand>(User user, TCommand command) where TCommand : ICommand
        {
            var handlerType = typeof(CommandHandlerBase<TCommand>);
            var handleMethod = handlerType.GetMethod("Handle");
            try
            {
                using (kernel.BeginScope())
                using (new ActivityScope(
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteCommandStart_3010,
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteCommandStop_3011,
                    typeof(TCommand).Name))
                using (new ModelContext(Trace.CorrelationManager.ActivityId))
                {
                    var commandAsJson = JsonConvert.SerializeObject(command, Formatting.Indented, settings);
                    object handler = null;
                    try
                    {
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

                        handler = kernel.Resolve(handlerType);
                        handleMethod.Invoke(handler, new[] { (object)command });

                        var whatChanged = documentSession.Advanced.WhatChanged();
                        if (whatChanged.Any())
                        {
                            TracingLogger.Information("Saving {0} changes", whatChanged.Count);
                            documentSession.SaveChanges();
                        }
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