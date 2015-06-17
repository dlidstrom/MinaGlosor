using JetBrains.Annotations;
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

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private PracticeSessionCreatedEvent()
#pragma warning restore 612, 618
        {
        }

        public string WordListId { get; set; }

        public PracticeWord[] Words { get; set; }

        public string OwnerId { get; set; }
    }
}