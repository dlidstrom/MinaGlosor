using System;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class MarkCreateAccountRequestUsedEvent : ModelEvent
    {
        public MarkCreateAccountRequestUsedEvent(DateTime date)
        {
            Date = date;
        }

        [JsonConstructor]
        private MarkCreateAccountRequestUsedEvent()
        {
        }

        public DateTime Date { get; private set; }
    }
}