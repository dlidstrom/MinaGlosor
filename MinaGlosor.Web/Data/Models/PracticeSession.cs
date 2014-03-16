using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Data.Models
{
    public class PracticeSession
    {
        public PracticeSession(WordList wordList)
        {
            WordList = wordList;
            WordListId = wordList.Id;
            ValidFrom = SystemTime.UtcNow;
        }

        public DateTime ValidFrom { get; private set; }

        public DateTime? ValidTo { get; private set; }

        public WordList WordList { get; private set; }

        public int WordListId { get; private set; }

        public void MarkAsDone()
        {
            ValidTo = SystemTime.UtcNow;
        }
    }
}