namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordRegisteredEvent : ModelEvent
    {
        public WordRegisteredEvent(string wordListId, string modelId, string text, string definition)
            : base(modelId)
        {
            WordListId = wordListId;
            Text = text;
            Definition = definition;
        }

        public string WordListId { get; private set; }

        public string Text { get; private set; }

        public string Definition { get; private set; }
    }
}