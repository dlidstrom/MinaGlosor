using System;

namespace MinaGlosor.Web.Models.Domain.ProgressModel
{
    public class ProgressPercentages
    {
        public int PercentDone { get; private set; }

        public int PercentExpired { get; private set; }
        
        public int PercentEasyWords { get; private set; }

        public int PercentDifficultWords { get; private set; }

        public ProgressPercentages Of(ProgressWordCounts progressWordCounts, int numberOfWords)
        {
            int percentDifficultWords;
            var percentEasyWords = CalculateDifficultyPercents(progressWordCounts, out percentDifficultWords);

            var percentDone = CalculatePercent(progressWordCounts.NumberOfWordScores, numberOfWords);
            var percentExpired = CalculatePercent(progressWordCounts.NumberOfWordsExpired, numberOfWords);
            return new ProgressPercentages
            {
                PercentDone = percentDone,
                PercentExpired = percentExpired,
                PercentEasyWords = percentEasyWords,
                PercentDifficultWords = percentDifficultWords
            };
        }

        public ProgressPercentages Difficulties(ProgressWordCounts progressWordCounts)
        {
            int percentDifficultWords;
            var percentEasyWords = CalculateDifficultyPercents(progressWordCounts, out percentDifficultWords);

            return new ProgressPercentages
            {
                PercentDone = PercentDone,
                PercentExpired = PercentExpired,
                PercentEasyWords = percentEasyWords,
                PercentDifficultWords = percentDifficultWords
            };
        }

        private static int CalculatePercent(int numberOfWordScores, int numberOfWords)
        {
            double temp;
            return CalculatePercent(numberOfWordScores, numberOfWords, out temp);
        }

        private static int CalculatePercent(int wordScores, int numberOfWords, out double fraction)
        {
            var percentDone = 100.0 * wordScores / Math.Max(1, numberOfWords);
            var result = (int)Math.Floor(percentDone);
            fraction = percentDone - result;
            return result;
        }

        private int CalculateDifficultyPercents(ProgressWordCounts progressWordCounts, out int percentDifficultWords)
        {
            double easyWordsFraction;
            var percentEasyWords = CalculatePercent(
                progressWordCounts.NumberOfEasyWords,
                progressWordCounts.NumberOfWordScores,
                out easyWordsFraction);

            double difficultWordsFraction;
            percentDifficultWords = CalculatePercent(
                progressWordCounts.NumberOfDifficultWords,
                progressWordCounts.NumberOfWordScores,
                out difficultWordsFraction);

            if (PercentDone == 100 && percentEasyWords + percentDifficultWords < 100)
            {
                if (easyWordsFraction >= difficultWordsFraction)
                {
                    percentEasyWords++;
                }
                else
                {
                    percentDifficultWords++;
                }
            }

            return percentEasyWords;
        }
    }
}