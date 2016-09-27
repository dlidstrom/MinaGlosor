using System;
using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class CheckIfWordExpiresEvent : ModelEvent
    {
        public CheckIfWordExpiresEvent(string id, DateTime repeatAfterDate, WordDifficultyUpdate wordDifficultyUpdate)
            : base(id)
        {
            RepeatAfterDate = repeatAfterDate;
            WordDifficultyUpdate = wordDifficultyUpdate;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private CheckIfWordExpiresEvent()
#pragma warning restore 612, 618
        {
        }

        public DateTime RepeatAfterDate { get; private set; }
        public WordDifficultyUpdate WordDifficultyUpdate { get; private set; }
    }
}