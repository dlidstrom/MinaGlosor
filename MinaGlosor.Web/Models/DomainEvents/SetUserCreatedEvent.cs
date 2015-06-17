using System;
using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class SetUserCreatedEvent : ModelEvent
    {
        public SetUserCreatedEvent(string id, DateTime createdDate)
            : base(id)
        {
            CreatedDate = createdDate;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private SetUserCreatedEvent()
#pragma warning restore 612, 618
        {
        }

        public DateTime CreatedDate { get; private set; }
    }
}