using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class UpdateLastPickedDateEvent : ModelEvent
    {
        public UpdateLastPickedDateEvent(string id, string practiceWordId)
            : base(id)
        {
            PracticeWordId = practiceWordId;
        }

        [JsonConstructor]
        private UpdateLastPickedDateEvent()
        {
        }

        public string PracticeWordId { get; private set; }
    }
}