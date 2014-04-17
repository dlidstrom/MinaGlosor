using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Data.Models
{
    public class PracticeSession
    {
        [Obsolete("User.Practice")]
        public PracticeSession(WordList wordList, User owner)
        {
            if (wordList == null) throw new ArgumentNullException("wordList");
            if (owner == null) throw new ArgumentNullException("owner");
            WordListId = wordList.Id;
            WordList = wordList;
            OwnerId = owner.Id;
            Owner = owner;
            ValidFrom = SystemTime.UtcNow;
            PracticeWords = new Collection<PracticeWord>();
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

        public int OwnerId { get; private set; }

        public virtual User Owner { get; private set; }

        public virtual ICollection<PracticeWord> PracticeWords { get; private set; }

        public void AddPracticeWord(WordScore wordScore)
        {
            if (wordScore == null) throw new ArgumentNullException("wordScore");
            PracticeWords.Add(new PracticeWord(this, wordScore));
        }

        public void MarkAsDone()
        {
            ValidTo = SystemTime.UtcNow;
        }
    }
}