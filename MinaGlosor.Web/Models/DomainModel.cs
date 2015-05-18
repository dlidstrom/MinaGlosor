using System;
using System.Collections.Generic;
using System.Reflection;
using MinaGlosor.Web.Models.DomainEvents;

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

        public string Id { get; private set; }

        public List<object> Events { get; protected set; }

        protected void Apply(object @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var type = GetType();
            var methodInfo = type.GetMethod(
                "ApplyEvent",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { @event.GetType() },
                null);
            methodInfo.Invoke(this, new[] { @event });
            Events.Add(@event);

            // signal event using DomainEvent class
            DomainEvent.Raise(@event);
        }
    }
}