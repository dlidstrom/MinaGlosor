using System;

namespace MinaGlosor.Web.Data.Models
{
    public class PracticeWord
    {
        public PracticeWord(PracticeSession practiceSession, WordScore wordScore)
        {
            if (wordScore == null) throw new ArgumentNullException("wordScore");
            WordScore = wordScore;
            WordScoreId = wordScore.Id;
            PracticeSessionId = practiceSession.Id;
            PracticeSession = practiceSession;
        }

        public int Id { get; set; }

        public int WordScoreId { get; private set; }

        public virtual WordScore WordScore { get; private set; }

        public int PracticeSessionId { get; private set; }

        public virtual PracticeSession PracticeSession { get; private set; }
    }
}