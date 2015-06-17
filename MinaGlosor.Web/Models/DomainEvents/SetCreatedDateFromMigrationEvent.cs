using System;
using JetBrains.Annotations;
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

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private SetCreatedDateFromMigrationEvent()
#pragma warning restore 612, 618
        {
        }

        public DateTime CreatedDateFromMigration { get; private set; }
    }
}