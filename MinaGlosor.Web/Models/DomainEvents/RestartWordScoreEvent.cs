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

        [JsonConstructor]
        private RestartWordScoreEvent()
        {
        }

        public int IntervalInDays { get; private set; }

        public int Count { get; private set; }

        public int TimesForgotten { get; private set; }
    }
}