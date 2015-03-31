using System;

namespace MinaGlosor.Web.Models
{
    public class PracticeSessionStatistics
    {
        public PracticeSessionStatistics(
            bool isFinished,
            int numberOfWords,
            int numberOfWordsEasilyLearnt,
            int numberOfWordsRecalledWithDifficulty,
            int numberOfWordsForgotten)
        {
            IsFinished = isFinished;
            NumberOfWords = numberOfWords;
            NumberOfWordsEasilyLearnt = numberOfWordsEasilyLearnt;
            NumberOfWordsRecalledWithDifficulty = numberOfWordsRecalledWithDifficulty;
            NumberOfWordsForgotten = numberOfWordsForgotten;
            var safeNumberOfWords = Math.Max(1, numberOfWords);
            Green = (int)Math.Floor(100.0 * NumberOfWordsEasilyLearnt / safeNumberOfWords);
            Blue = (int)Math.Floor(100.0 * NumberOfWordsRecalledWithDifficulty / safeNumberOfWords);
            Yellow = (int)Math.Floor(100.0 * NumberOfWordsForgotten / safeNumberOfWords);

            if (numberOfWords == numberOfWordsEasilyLearnt + numberOfWordsRecalledWithDifficulty + numberOfWordsForgotten)
            {
                if (numberOfWordsForgotten > 0)
                    Yellow = 100 - (Green + Blue);
                else if (numberOfWordsRecalledWithDifficulty > 0)
                    Blue = 100 - Green;
            }
        }

        public bool IsFinished { get; private set; }

        public int NumberOfWords { get; private set; }

        public int NumberOfWordsEasilyLearnt { get; private set; }

        public int NumberOfWordsRecalledWithDifficulty { get; private set; }

        public int NumberOfWordsForgotten { get; private set; }

        public int Green { get; private set; }

        public int Blue { get; private set; }

        public int Yellow { get; private set; }
    }
}