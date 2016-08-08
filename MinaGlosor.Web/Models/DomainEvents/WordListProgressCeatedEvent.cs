using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordListProgressCeatedEvent : ModelEvent
    {
        public WordListProgressCeatedEvent(
            string modelId,
            string ownerId,
            string wordListId)
            : base(modelId)
        {
            OwnerId = ownerId;
            WordListId = wordListId;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private WordListProgressCeatedEvent()
#pragma warning restore 612, 618
        {
        }

        public string OwnerId { get; private set; }

        public string WordListId { get; private set; }
    }
}