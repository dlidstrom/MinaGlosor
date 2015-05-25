using System;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class UpdateLastPickedDateEvent : ModelEvent
    {
        public UpdateLastPickedDateEvent(string id, string practiceWordId, DateTime date)
            : base(id)
        {
            PracticeWordId = practiceWordId;
            Date = date;
        }

        [JsonConstructor]
        private UpdateLastPickedDateEvent()
        {
        }

        public string PracticeWordId { get; private set; }

        public DateTime Date { get; private set; }
    }
}