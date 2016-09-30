using System;
using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class UpdateWordScoreEvent : ModelEvent
    {
        public UpdateWordScoreEvent(string id, string wordId, string ownerId, int count, int intervalInDays, DateTime repeatAfterDate, double score, WordDifficulty wordDifficulty, WordScoreDifficultyLifecycle wordScoreDifficultyLifecycle)
            : base(id)
        {
            WordId = wordId;
            OwnerId = ownerId;
            Count = count;
            IntervalInDays = intervalInDays;
            RepeatAfterDate = repeatAfterDate;
            Score = score;
            WordDifficulty = wordDifficulty;
            WordScoreDifficultyLifecycle = wordScoreDifficultyLifecycle;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private UpdateWordScoreEvent()
#pragma warning restore 612, 618
        {
        }

        public string WordId { get; private set; }

        public string OwnerId { get; private set; }
        
        public int Count { get; private set; }

        public int IntervalInDays { get; private set; }

        public DateTime RepeatAfterDate { get; private set; }

        public double Score { get; private set; }

        public WordDifficulty WordDifficulty { get; private set; }
        
        public WordScoreDifficultyLifecycle WordScoreDifficultyLifecycle { get; private set; }
    }
}