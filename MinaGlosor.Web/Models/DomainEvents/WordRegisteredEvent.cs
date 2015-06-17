using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordRegisteredEvent : ModelEvent
    {
        public WordRegisteredEvent(string userId, string wordListId, string modelId, string text, string definition)
            : base(modelId)
        {
            UserId = userId;
            WordListId = wordListId;
            Text = text;
            Definition = definition;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private WordRegisteredEvent()
#pragma warning restore 612, 618
        {
        }

        public string UserId { get; private set; }

        public string WordListId { get; private set; }

        public string Text { get; private set; }

        public string Definition { get; private set; }
    }
}