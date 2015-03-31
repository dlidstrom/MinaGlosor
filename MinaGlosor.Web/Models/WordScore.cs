using System;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Abstractions;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class WordScore
    {
        public WordScore(string ownerId, string wordId, string wordListId)
        {
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (wordListId == null) throw new ArgumentNullException("wordListId");

            OwnerId = ownerId;
            WordId = wordId;
            WordListId = wordListId;
            Score = 2.5;
        }

        [JsonConstructor]
        private WordScore()
        {
        }

        public string OwnerId { get; private set; }

        public string WordId { get; private set; }

        public string WordListId { get; private set; }

        public int Count { get; private set; }

        public double Score { get; private set; }

        public int IntervalInDays { get; private set; }

        public DateTime RepeatAfterDate { get; private set; }

        public int TimesForgotten { get; private set; }

        public void ScoreWord(ConfidenceLevel confidenceLevel)
        {
            switch (confidenceLevel)
            {
                case ConfidenceLevel.PerfectResponse:
                case ConfidenceLevel.CorrectAfterHesitation:
                case ConfidenceLevel.RecalledWithSeriousDifficulty:
                case ConfidenceLevel.IncorrectWithEasyRecall:
                case ConfidenceLevel.IncorrectButRemembered:
                case ConfidenceLevel.CompleteBlackout:
                    {
                        var level = (int)confidenceLevel;
                        if (level < 3)
                        {
                            IntervalInDays = 1;
                            Count = 1;
                            TimesForgotten++;
                        }
                        else
                        {
                            if (level >= 4)
                                DomainEvent.Raise(new WordRememberedEvent(OwnerId));

                            if (Count > 0 && SystemTime.UtcNow < RepeatAfterDate)
                            {
                                if (level >= 4) return;
                                Count = 0;
                            }

                            Count++;
                            if (Count == 1)
                            {
                                IntervalInDays = 1;
                            }
                            else if (Count == 2)
                            {
                                IntervalInDays = 6;
                            }
                            else
                            {
                                IntervalInDays = (int)Math.Ceiling(Score * IntervalInDays);
                            }
                        }

                        RepeatAfterDate = SystemTime.UtcNow.AddDays(IntervalInDays);
                        var newScore = Score + (0.1 - (5 - level) * (0.08 + (5 - level) * 0.02));
                        newScore = Math.Max(1.3, newScore);
                        Score = newScore;

                        break;
                    }

                default:
                    throw new ApplicationException("Unknown confidence level: " + confidenceLevel);
            }
        }
    }
}