using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class RestartWordScoreEvent : ModelEvent
    {
        public RestartWordScoreEvent(string id, int intervalInDays, int count, int timesForgotten)
            : base(id)
        {
            IntervalInDays = intervalInDays;
            Count = count;
            TimesForgotten = timesForgotten;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private RestartWordScoreEvent()
#pragma warning restore 612, 618
        {
        }

        public int IntervalInDays { get; private set; }

        public int Count { get; private set; }

        public int TimesForgotten { get; private set; }
    }
}