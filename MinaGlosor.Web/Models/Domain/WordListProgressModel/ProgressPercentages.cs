using System;

namespace MinaGlosor.Web.Models.Domain.WordListProgressModel
{
    public class ProgressPercentages
    {
        public int PercentDone { get; private set; }

        public int PercentExpired { get; private set; }
        
        public int PercentEasyWords { get; private set; }

        public int PercentDifficultWords { get; private set; }

        public ProgressPercentages Of(ProgressWordCounts progressWordCounts, int numberOfWords)
        {
            double easyWordsFraction;
            var percentEasyWords = CalculatePercent(progressWordCounts.NumberOfEasyWords, numberOfWords, out easyWordsFraction);

            double difficultWordsFraction;
            var percentDifficultWords = CalculatePercent(progressWordCounts.NumberOfDifficultWords, numberOfWords, out difficultWordsFraction);

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

        private int CalculatePercent(int numberOfWordScores, int numberOfWords)
        {
            double temp;
            return CalculatePercent(numberOfWordScores, numberOfWords, out temp);
        }

        public ProgressPercentages Difficulties(ProgressWordCounts progressWordCounts)
        {
            // fortsätt här
            return new ProgressPercentages
            {
            };
        }

        private static int CalculatePercent(int wordScores, int numberOfWords, out double fraction)
        {
            var percentDone = Math.Floor(100.0 * wordScores / Math.Max(1, numberOfWords));
            var result = (int)percentDone;
            fraction = percentDone - result;
            return result;
        }
    }
}