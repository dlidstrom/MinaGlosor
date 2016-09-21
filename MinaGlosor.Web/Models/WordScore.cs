﻿using System;
using JetBrains.Annotations;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Abstractions;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class WordScore : DomainModel
    {
        private const double DefaultScore = 2.5;
        private const WordDifficulty DefaultWordDifficulty = WordDifficulty.NotYetScored;

        // TODO: Id can be computed from ownerId and wordId
        public WordScore(string id, string ownerId, string wordId, string wordListId)
            : base(id)
        {
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (wordListId == null) throw new ArgumentNullException("wordListId");

            Apply(new WordScoreRegisteredEvent(id, ownerId, wordId, wordListId, DefaultScore, DefaultWordDifficulty));
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private WordScore()
#pragma warning restore 612, 618
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

        public bool SignalledWordExpired { get; private set; }

        public WordDifficulty WordDifficulty { get; private set; }

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
                    break;

                default:
                    throw new ApplicationException("Unknown confidence level: " + confidenceLevel);
            }

            var count = Count;
            int intervalInDays;
            var level = (int)confidenceLevel;
            var utcNow = SystemTime.UtcNow;
            if (level < 3)
            {
                intervalInDays = 1;
                count = 1;
                Apply(new RestartWordScoreEvent(Id, intervalInDays, count, TimesForgotten + 1));
            }
            else
            {
                if (count > 0 && utcNow < RepeatAfterDate)
                {
                    if (level >= 4)
                    {
                        TracingLogger.Information(
                            EventIds.Informational_ApplicationLog_3XXX.Web_ScoreNotUpdated_3003,
                            "Not updated due to: Now {0} < RepeatAfterDate {1}",
                            utcNow,
                            RepeatAfterDate);
                        return;
                    }

                    count = 0;
                }

                switch (++count)
                {
                    case 1:
                        intervalInDays = 1;
                        break;

                    case 2:
                        intervalInDays = 6;
                        break;

                    default:
                        intervalInDays = (int)Math.Ceiling(Score * IntervalInDays);
                        break;
                }
            }

            var repeatAfterDate = utcNow.AddDays(intervalInDays);
            var score = Math.Max(1.3, Score + (0.1 - (5 - level) * (0.08 + (5 - level) * 0.02)));
            var oldRepeatAfterDate = RepeatAfterDate;
            Apply(new UpdateWordScoreEvent(Id, count, intervalInDays, repeatAfterDate, score));
            Apply(new CheckIfWordExpiresEvent(Id, repeatAfterDate));
            if (oldRepeatAfterDate != default(DateTime) && oldRepeatAfterDate < SystemTime.UtcNow)
            {
                // word was expired, now is up to date
                Apply(new WordIsUpToDateEvent(Id, WordId, OwnerId));
            }
        }

        public void ResetAfterWordEdit()
        {
            const int NewIntervalInDays = 0;
            const int NewCount = 0;
            const double NewScore = DefaultScore;
            var repeatAfterDate = SystemTime.UtcNow.AddDays(NewIntervalInDays);
            Apply(new RestartAfterEditEvent(Id, NewIntervalInDays, NewCount, repeatAfterDate, NewScore));
            Apply(new CheckIfWordExpiresEvent(Id, repeatAfterDate));
        }

        public void CheckIfWordExpires()
        {
            var utcNow = SystemTime.UtcNow;
            if (SignalledWordExpired == false && RepeatAfterDate < utcNow)
            {
                Apply(new WordExpiredEvent(Id, WordId, OwnerId));
            }
        }

        public void UpdateDifficultyAfterPractice(ConfidenceLevel[] confidenceLevels)
        {
            WordScoreChangedDifficultyEvent @event = null;
            var firstAnswer = confidenceLevels[0];
            switch (WordDifficulty)
            {
                case WordDifficulty.Difficult:
                {
                    if (firstAnswer >= ConfidenceLevel.CorrectAfterHesitation)
                    {
                        @event = new WordScoreChangedDifficultyEvent(
                            Id,
                            WordListId,
                            OwnerId,
                            WordDifficulty.Easy,
                            WordScoreDifficultyLifecycle.TurnedEasy);
                    }
                    else
                    {
                        // still difficult
                    }

                    break;
                }

                case WordDifficulty.Easy:
                {
                    if (firstAnswer < ConfidenceLevel.CorrectAfterHesitation)
                    {
                        @event = new WordScoreChangedDifficultyEvent(
                            Id,
                            WordListId,
                            OwnerId,
                            WordDifficulty.Difficult,
                            WordScoreDifficultyLifecycle.TurnedDifficult);
                    }
                    else
                    {
                        // still easy
                    }

                    break;
                }

                case WordDifficulty.NotYetScored:
                {
                    if (firstAnswer >= ConfidenceLevel.CorrectAfterHesitation)
                    {
                        @event = new WordScoreChangedDifficultyEvent(
                            Id,
                            WordListId,
                            OwnerId,
                            WordDifficulty.Easy,
                            WordScoreDifficultyLifecycle.FirstTimeEasy);
                    }
                    else
                    {
                        @event = new WordScoreChangedDifficultyEvent(
                            Id,
                            WordListId,
                            OwnerId,
                            WordDifficulty.Difficult,
                            WordScoreDifficultyLifecycle.FirstTimeDifficult);
                    }

                    break;
                }
            }

            if (@event != null)
            {
                Apply(@event);
            }
        }

        private void ApplyEvent(WordScoreRegisteredEvent @event)
        {
            OwnerId = @event.OwnerId;
            WordId = @event.WordId;
            WordListId = @event.WordListId;
            Score = @event.Score;
        }

        private void ApplyEvent(RestartWordScoreEvent @event)
        {
            IntervalInDays = @event.IntervalInDays;
            Count = @event.Count;
            TimesForgotten = @event.TimesForgotten;
        }

        private void ApplyEvent(UpdateWordScoreEvent @event)
        {
            Count = @event.Count;
            IntervalInDays = @event.IntervalInDays;
            RepeatAfterDate = @event.RepeatAfterDate;
            Score = @event.Score;
            SignalledWordExpired = false;
        }

        private void ApplyEvent(RestartAfterEditEvent @event)
        {
            Count = @event.Count;
            IntervalInDays = @event.IntervalInDays;
            RepeatAfterDate = @event.RepeatAfterDate;
            Score = @event.Score;
            SignalledWordExpired = false;
        }

        private void ApplyEvent(WordExpiredEvent @event)
        {
            SignalledWordExpired = true;
        }

        private void ApplyEvent(CheckIfWordExpiresEvent @event)
        {
            // to let the world know about the date
        }

        private void ApplyEvent(WordIsUpToDateEvent @event)
        {
            // to let the world know
        }

        private void ApplyEvent(UpdateWordScoreDifficultyEvent @event)
        {
            WordDifficulty = @event.WordDifficulty;
        }

        private void ApplyEvent(WordScoreChangedDifficultyEvent @event)
        {
            WordDifficulty = @event.WordDifficulty;
        }
    }
}