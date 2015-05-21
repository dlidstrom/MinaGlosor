using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class UpdateConfidenceEvent : ModelEvent
    {
        public UpdateConfidenceEvent(string id, string practiceWordId, ConfidenceLevel confidenceLevel)
            : base(id)
        {
            PracticeWordId = practiceWordId;
            ConfidenceLevel = confidenceLevel;
        }

        [JsonConstructor]
        private UpdateConfidenceEvent()
        {
        }

        public string PracticeWordId { get; private set; }

        public ConfidenceLevel ConfidenceLevel { get; private set; }
    }
}