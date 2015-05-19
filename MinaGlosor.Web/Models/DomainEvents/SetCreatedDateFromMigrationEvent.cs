using System;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class SetCreatedDateFromMigrationEvent : ModelEvent
    {
        public SetCreatedDateFromMigrationEvent(string modelId, DateTime createdDate, Guid correlationId, Guid? causationId)
            : base(modelId, correlationId, causationId)
        {
            CreatedDateFromMigration = createdDate;
        }

        public DateTime CreatedDateFromMigration { get; private set; }
    }
}