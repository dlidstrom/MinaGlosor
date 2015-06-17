using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordUpdatedEvent : ModelEvent
    {
        public WordUpdatedEvent(string wordId, string text, string definition, string wordListId)
            : base(wordId)
        {
            Text = text;
            Definition = definition;
            WordListId = wordListId;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private WordUpdatedEvent()
#pragma warning restore 612, 618
        {
        }

        public string Text { get; private set; }

        public string Definition { get; private set; }

        public string WordListId { get; private set; }
    }
}