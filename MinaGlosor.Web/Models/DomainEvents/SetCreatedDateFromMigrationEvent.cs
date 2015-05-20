using System;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class SetCreatedDateFromMigrationEvent : ModelEvent
    {
        public SetCreatedDateFromMigrationEvent(string modelId, DateTime createdDate)
            : base(modelId)
        {
            CreatedDateFromMigration = createdDate;
        }

        [JsonConstructor]
        private SetCreatedDateFromMigrationEvent()
        {
        }

        public DateTime CreatedDateFromMigration { get; private set; }
    }
}