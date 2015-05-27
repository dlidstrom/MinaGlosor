using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class AddWordEvent : ModelEvent
    {
        public AddWordEvent(string id, int numberOfWords)
            : base(id)
        {
            NumberOfWords = numberOfWords;
        }

        [JsonConstructor]
        private AddWordEvent()
        {
        }

        public int NumberOfWords { get; private set; }
    }
}