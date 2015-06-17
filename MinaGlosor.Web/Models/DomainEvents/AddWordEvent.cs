using JetBrains.Annotations;
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

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private AddWordEvent()
#pragma warning restore 612, 618
        {
        }

        public int NumberOfWords { get; private set; }
    }
}