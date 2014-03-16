using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Data.Models
{
    public class PracticeSession
    {
        public PracticeSession(WordList wordList, IList<PracticeWord> practiceWords)
        {
            if (wordList == null) throw new ArgumentNullException("wordList");
            if (practiceWords == null) throw new ArgumentNullException("practiceWords");
            WordList = wordList;
            WordListId = wordList.Id;
            ValidFrom = SystemTime.UtcNow;
            PracticeWords = new Collection<PracticeWord>(practiceWords);
        }

        private PracticeSession()
        {
            PracticeWords = new Collection<PracticeWord>();
        }

        public int Id { get; set; }

        public DateTime ValidFrom { get; private set; }

        public DateTime? ValidTo { get; private set; }

        public int WordListId { get; private set; }

        public virtual WordList WordList { get; private set; }

        public virtual ICollection<PracticeWord> PracticeWords { get; private set; }

        public void MarkAsDone()
        {
            ValidTo = SystemTime.UtcNow;
        }
    }
}