using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class PracticeSessionCreatedEvent : ModelEvent
    {
        public PracticeSessionCreatedEvent(string id, string wordListId, PracticeWord[] words, string ownerId)
            : base(id)
        {
            WordListId = wordListId;
            Words = words;
            OwnerId = ownerId;
        }

        [JsonConstructor]
        private PracticeSessionCreatedEvent()
        {
        }

        public string WordListId { get; set; }

        public PracticeWord[] Words { get; set; }

        public string OwnerId { get; set; }
    }
}