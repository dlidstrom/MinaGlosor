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

        [JsonConstructor]
        private WordListRegisteredEvent()
        {
        }

        public string Name { get; private set; }

        public string OwnerId { get; private set; }
    }
}