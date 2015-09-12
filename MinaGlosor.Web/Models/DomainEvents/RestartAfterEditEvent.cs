using System;
using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class RestartAfterEditEvent : ModelEvent
    {
        public RestartAfterEditEvent(string id, int intervalInDays, int count, DateTime repeatAfterDate)
            : base(id)
        {
            IntervalInDays = intervalInDays;
            Count = count;
            RepeatAfterDate = repeatAfterDate;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private RestartAfterEditEvent()
#pragma warning restore 612, 618
        {
        }

        public int IntervalInDays { get; private set; }

        public int Count { get; private set; }

        public DateTime RepeatAfterDate { get; private set; }
    }
}