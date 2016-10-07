namespace MinaGlosor.Web.Models.Domain.ProgressModel
{
    public class ProgressWordCounts
    {
        public int NumberOfWordScores { get; private set; }

        public int NumberOfWordsExpired { get; private set; }

        public int NumberOfEasyWords { get; private set; }

        public int NumberOfDifficultWords { get; private set; }

        public ProgressWordCounts IncreaseExpired()
        {
            return new ProgressWordCounts
            {
                NumberOfWordScores = NumberOfWordScores,
                NumberOfWordsExpired = NumberOfWordsExpired + 1,
                NumberOfEasyWords = NumberOfEasyWords,
                NumberOfDifficultWords = NumberOfDifficultWords
            };
        }

        public ProgressWordCounts DecreaseExpired()
        {
            return new ProgressWordCounts
            {
                NumberOfWordScores = NumberOfWordScores,
                NumberOfWordsExpired = NumberOfWordsExpired - 1,
                NumberOfEasyWords = NumberOfEasyWords,
                NumberOfDifficultWords = NumberOfDifficultWords
            };
        }

        public ProgressWordCounts IncreaseCount()
        {
            return new ProgressWordCounts
            {
                NumberOfWordScores = NumberOfWordScores + 1,
                NumberOfWordsExpired = NumberOfWordsExpired,
                NumberOfEasyWords = NumberOfEasyWords,
                NumberOfDifficultWords = NumberOfDifficultWords
            };
        }

        public ProgressWordCounts IncreaseDifficult()
        {
            return new ProgressWordCounts
            {
                NumberOfWordScores = NumberOfWordScores,
                NumberOfWordsExpired = NumberOfWordsExpired,
                NumberOfEasyWords = NumberOfEasyWords,
                NumberOfDifficultWords = NumberOfDifficultWords + 1
            };
        }

        public ProgressWordCounts TurnedDifficult()
        {
            return new ProgressWordCounts
            {
                NumberOfWordScores = NumberOfWordScores,
                NumberOfWordsExpired = NumberOfWordsExpired,
                NumberOfEasyWords = NumberOfEasyWords - 1,
                NumberOfDifficultWords = NumberOfDifficultWords + 1
            };
        }

        public ProgressWordCounts IncreaseEasy()
        {
            return new ProgressWordCounts
            {
                NumberOfWordScores = NumberOfWordScores,
                NumberOfWordsExpired = NumberOfWordsExpired,
                NumberOfEasyWords = NumberOfEasyWords + 1,
                NumberOfDifficultWords = NumberOfDifficultWords
            };
        }

        public ProgressWordCounts TurnedEasy()
        {
            return new ProgressWordCounts
            {
                NumberOfWordScores = NumberOfWordScores,
                NumberOfWordsExpired = NumberOfWordsExpired,
                NumberOfEasyWords = NumberOfEasyWords + 1,
                NumberOfDifficultWords = NumberOfDifficultWords - 1
            };
        }
    }
}