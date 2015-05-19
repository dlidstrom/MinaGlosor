using System;
using System.Web;
using Castle.Windsor;

namespace MinaGlosor.Web.Models.DomainEvents
{
    /// <summary>
    /// Used to raise events within the domain model.
    /// </summary>
    public static class DomainEvent
    {
        private static Guid correlationId;

        public static Guid CorrelationId
        {
            get
            {
                Guid? id;
                if (HttpContext.Current != null)
                {
                    id = HttpContext.Current.Items["CorrelationId"] as Guid?;
                }
                else
                {
                    id = correlationId;
                }

                if (id.GetValueOrDefault() == default(Guid))
                {
                    throw new ApplicationException("Forgot to call DomainEvent.Correlate?");
                }

                return id.GetValueOrDefault();
            }

            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items["CorrelationId"] = value;
                }
                else
                {
                    correlationId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets an action used to raise events.
        /// Used for testing purposes.
        /// </summary>
        private static Action<object> RaiseAction { get; set; }

        /// <summary>
        /// Gets or sets a function that supplies the container.
        /// Can be used to set a container for tests.
        /// </summary>
        private static Func<IWindsorContainer> ReturnContainer { get; set; }

        public static void SetContainer(IWindsorContainer container)
        {
            ReturnContainer = () => container;
        }

        /// <summary>
        /// Raises a domain event. Default action is to resolve all handlers
        /// and let them handle the event.
        /// </summary>
        /// <typeparam name="TEvent">Type of event.</typeparam>
        /// <param name="event">Event to raise.</param>
        public static void Raise<TEvent>(TEvent @event)
        {
            if (RaiseAction != null)
            {
                RaiseAction(@event);
                return;
            }

            var container = ReturnContainer.Invoke();
            if (container == null) throw new InvalidOperationException("container is null");
            var handlerType = typeof(IHandle<>).MakeGenericType(@event.GetType());
            var handlers = container.ResolveAll(handlerType);

            foreach (var handle in handlers)
            {
                var method = handle.GetType().GetMethod("Handle");
                method.Invoke(handle, new[] { (object)@event });
                container.Release(handle);
            }
        }

        /// <summary>
        /// Used for testing purposes.
        /// </summary>
        /// <param name="raiseAction">Custom raise action.</param>
        /// <returns>Domain reset event to go back to default behaviour.</returns>
        public static DomainEventReset TestWith(Action<object> raiseAction)
        {
            RaiseAction = raiseAction;

            return new DomainEventReset();
        }

        /// <summary>
        /// Resets the raise action to default behaviour.
        /// </summary>
        public static void Reset()
        {
            RaiseAction = null;
        }

        /// <summary>
        /// Used for testing purposes. Use within a using-scope:
        /// <code>
        /// using (DomainEvent.Disable())
        /// { ... }
        /// </code>
        /// No events will be raised within the using-scope.
        /// </summary>
        /// <returns>Event reset to default behaviour.</returns>
        public static DomainEventReset Disable()
        {
            RaiseAction = e => { };

            return new DomainEventReset();
        }

        public static IDisposable Correlate(Guid correlationId)
        {
            var previous = HttpContext.Current.Items["CorrelationId"] as Guid?;
            HttpContext.Current.Items["CorrelationId"] = correlationId;
            return new Reverter("CorrelationId", previous);
        }

        private class Reverter : IDisposable
        {
            private readonly string key;
            private readonly Guid? previous;

            public Reverter(string key, Guid? previous)
            {
                this.key = key;
                this.previous = previous;
            }

            public void Dispose()
            {
                HttpContext.Current.Items[key] = previous;
            }
        }
    }
}