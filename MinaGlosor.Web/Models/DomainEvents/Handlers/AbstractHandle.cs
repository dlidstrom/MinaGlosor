using System;
using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models.BackgroundTasks;
using Raven.Abstractions;
using Raven.Client;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public abstract class AbstractHandle<TEvent> : IHandle<TEvent>
    {
        public IKernel Kernel { get; set; }

        public QueryExecutor QueryExecutor { get; set; }

        public abstract void Handle(TEvent ev);

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            var result = QueryExecutor.ExecuteQuery(query, null);
            return result;
        }

        protected void SendTask<TBody>(TBody body, ModelEvent causedByEvent, DateTimeOffset? nextTry = null) where TBody : class
        {
            if (body == null) throw new ArgumentNullException("body");
            if (causedByEvent == null) throw new ArgumentNullException("causedByEvent");

            var bodyAsJson = body.ToJson();
            TracingLogger.Information(
                EventIds.Informational_ApplicationLog_3XXX.Web_SendTask_3009,
                "{0} <- {1}: {2}",
                body.GetType().Name,
                causedByEvent.GetType().Name,
                bodyAsJson);
            var session = Kernel.Resolve<IDocumentSession>();
            var task = BackgroundTask.Create(
                ModelContext.CorrelationId,
                causedByEvent.EventId,
                body,
                nextTry.GetValueOrDefault(SystemTime.UtcNow));
            session.Store(task);
        }
    }
}