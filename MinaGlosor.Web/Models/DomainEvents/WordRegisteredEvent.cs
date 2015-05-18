using System;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordRegisteredEvent : ModelEvent
    {
        public WordRegisteredEvent(string wordListId, string modelId, Guid correlationId, Guid? causationId)
            : base(modelId, correlationId, causationId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");

            WordListId = wordListId;
        }

        public string WordListId { get; private set; }
    }
}