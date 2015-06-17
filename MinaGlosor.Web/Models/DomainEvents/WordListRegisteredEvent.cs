using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordListRegisteredEvent : ModelEvent
    {
        public WordListRegisteredEvent(string id, string name, string ownerId)
            : base(id)
        {
            Name = name;
            OwnerId = ownerId;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private WordListRegisteredEvent()
#pragma warning restore 612, 618
        {
        }

        public string Name { get; private set; }

        public string OwnerId { get; private set; }
    }
}