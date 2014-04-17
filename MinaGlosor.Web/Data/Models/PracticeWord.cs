using System;

namespace MinaGlosor.Web.Data.Models
{
    public class PracticeWord
    {
        public PracticeWord(WordScore wordScore)
        {
            if (wordScore == null) throw new ArgumentNullException("wordScore");
            WordScore = wordScore;
            WordScoreId = wordScore.Id;
        }

        public int Id { get; set; }

        public int WordScoreId { get; private set; }

        public virtual WordScore WordScore { get; private set; }
    }
}