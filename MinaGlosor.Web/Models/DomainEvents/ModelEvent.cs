using System;
using Raven.Abstractions;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public abstract class ModelEvent
    {
        /// <summary>
        /// If you are responding to a message, you copy its correlation id as your correlation id, its message id is your
        /// causation id. This allows you to see an entire conversation (correlation id) or to see what causes what (causation id).
        /// </summary>
        protected ModelEvent(string modelId, Guid correlationId, Guid? causationId)
        {
            if (modelId == null) throw new ArgumentNullException("modelId");
            ModelId = modelId;
            CorrelationId = correlationId;
            CausationId = causationId.GetValueOrDefault();
            EventId = Guid.NewGuid();
            CreatedDateTime = SystemTime.UtcNow;
        }

        public string ModelId { get; private set; }

        public Guid CorrelationId { get; private set; }

        public Guid CausationId { get; private set; }

        public Guid EventId { get; private set; }

        public DateTime CreatedDateTime { get; private set; }
    }
}