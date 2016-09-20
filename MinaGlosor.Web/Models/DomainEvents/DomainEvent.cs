using System;
using Castle.Windsor;
using MinaGlosor.Web.Models.DomainEvents.Handlers;

namespace MinaGlosor.Web.Models.DomainEvents
{
    /// <summary>
    /// Used to raise events within the domain model.
    /// </summary>
    public static class DomainEvent
    {
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
    }
}