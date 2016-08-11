using System;
using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class UpdateWordScoreEvent : ModelEvent
    {
        public UpdateWordScoreEvent(string id, int count, int intervalInDays, DateTime repeatAfterDate, double score)
            : base(id)
        {
            Count = count;
            IntervalInDays = intervalInDays;
            RepeatAfterDate = repeatAfterDate;
            Score = score;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private UpdateWordScoreEvent()
#pragma warning restore 612, 618
        {
        }

        public int Count { get; private set; }

        public int IntervalInDays { get; private set; }

        public DateTime RepeatAfterDate { get; private set; }

        public double Score { get; private set; }
    }
}