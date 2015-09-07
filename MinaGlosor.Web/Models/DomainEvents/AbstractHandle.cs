﻿using System;
using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models.BackgroundTasks;
using Newtonsoft.Json;
using Raven.Client;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public abstract class AbstractHandle<TEvent> : IHandle<TEvent>
    {
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new PrivateMembersContractResolver(),
            TypeNameHandling = TypeNameHandling.All
        };

        public IKernel Kernel { get; set; }

        public CommandExecutor CommandExecutor { get; set; }

        public abstract void Handle(TEvent ev);

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.Execute(GetDocumentSession());
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command, ModelEvent causedByEvent)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (causedByEvent == null) throw new ArgumentNullException("causedByEvent");
            using (new ModelContext(ModelContext.CorrelationId, causedByEvent.EventId))
            {
                var commandAsJson = JsonConvert.SerializeObject(command, Formatting.Indented, serializerSettings);
                TracingLogger.Information(
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteDependentCommand_3001,
                    "{0} <- {1}: {2}",
                    command.GetType().Name,
                    causedByEvent.GetType().Name,
                    commandAsJson);
                var result = CommandExecutor.ExecuteCommand(null, command);
                return result;
            }
        }

        protected void SendTask<TBody>(TBody body, ModelEvent causedByEvent) where TBody : class
        {
            if (body == null) throw new ArgumentNullException("body");
            if (causedByEvent == null) throw new ArgumentNullException("causedByEvent");

            var bodyAsJson = JsonConvert.SerializeObject(body, Formatting.Indented, serializerSettings);
            TracingLogger.Information(
                EventIds.Informational_ApplicationLog_3XXX.Web_SendTask_3009,
                "{0} <- {1}: {2}",
                body.GetType().Name,
                causedByEvent.GetType().Name,
                bodyAsJson);
            var session = GetDocumentSession();
            var task = BackgroundTask.Create(ModelContext.CorrelationId, causedByEvent.EventId, body);
            session.Store(task);
        }

        private IDocumentSession GetDocumentSession()
        {
            return Kernel.Resolve<IDocumentSession>();
        }
    }
}