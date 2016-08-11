using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordIsUpToDateEvent : ModelEvent
    {
        public WordIsUpToDateEvent(string id, string wordId, string ownerId)
            : base(id)
        {
            WordId = wordId;
            OwnerId = ownerId;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private WordIsUpToDateEvent()
#pragma warning restore 612, 618
        {
        }

        public string WordId { get; private set; }
        public string OwnerId { get; private set; }
    }
}