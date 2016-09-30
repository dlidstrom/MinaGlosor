using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordExpiredEvent : ModelEvent
    {
        public WordExpiredEvent(string id, string wordId, string ownerId)
            : base(id)
        {
            WordId = wordId;
            OwnerId = ownerId;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private WordExpiredEvent()
#pragma warning restore 612, 618
        {
        }

        public string WordId { get; private set; }
        public string OwnerId { get; private set; }
    }
}