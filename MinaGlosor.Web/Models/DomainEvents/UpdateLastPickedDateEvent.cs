using System;
using JetBrains.Annotations;
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

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private UpdateLastPickedDateEvent()
#pragma warning restore 612, 618
        {
        }

        public string PracticeWordId { get; private set; }

        public DateTime Date { get; private set; }
    }
}