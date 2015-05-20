using System;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class SetCreatedDateFromMigrationEvent : ModelEvent
    {
        public SetCreatedDateFromMigrationEvent(string modelId, DateTime createdDate)
            : base(modelId)
        {
            CreatedDateFromMigration = createdDate;
        }

        public DateTime CreatedDateFromMigration { get; private set; }
    }
}