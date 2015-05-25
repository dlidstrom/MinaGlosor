using System;
using System.Collections.Generic;
using System.Reflection;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models.DomainEvents;
using Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public abstract class DomainModel
    {
        protected DomainModel(string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            Id = id;
            Events = new List<object>();
        }

        [Obsolete("Use DomainModel(string id)")]
        protected DomainModel()
        {
            Events = new List<object>();
        }

        public string Id { get; protected set; }

        public List<object> Events { get; protected set; }

        protected void Apply(ModelEvent @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            TracingLogger.Information(
                EventIds.Informational_ApplicationLog_3XXX.Web_RaiseEvent_3002,
                JsonConvert.SerializeObject(@event, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));
            var type = GetType();
            var methodInfo = type.GetMethod(
                "ApplyEvent",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { @event.GetType() },
                null);
            methodInfo.Invoke(this, new[] { (object)@event });
            Events.Add(@event);

            // raise global event
            DomainEvent.Raise(@event);
        }
    }
}