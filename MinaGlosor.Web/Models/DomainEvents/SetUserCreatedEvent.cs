using System;
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

        [JsonConstructor]
        public SetUserCreatedEvent()
        {
        }

        public DateTime CreatedDate { get; private set; }
    }
}