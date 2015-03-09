using System;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordRegisteredEvent
    {
        public WordRegisteredEvent(string wordListId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");

            WordListId = wordListId;
        }

        public string WordListId { get; private set; }
    }
}