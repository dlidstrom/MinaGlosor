using System;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class MarkResetPasswordRequestUsedEvent : ModelEvent
    {
        public MarkResetPasswordRequestUsedEvent(string id, DateTime date)
            : base(id)
        {
            Date = date;
        }

        [JsonConstructor]
        private MarkResetPasswordRequestUsedEvent()
        {
        }

        public DateTime Date { get; private set; }
    }
}