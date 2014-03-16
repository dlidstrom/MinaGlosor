using System;

namespace MinaGlosor.Web.Data.Models
{
    public class PracticeWord
    {
        public PracticeWord(Word word)
        {
            if (word == null) throw new ArgumentNullException("word");
            Word = word;
            WordId = word.Id;
        }

        public virtual Word Word { get; private set; }

        public int WordId { get; private set; }
    }
}