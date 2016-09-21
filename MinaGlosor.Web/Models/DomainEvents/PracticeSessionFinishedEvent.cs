using System;
using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class PracticeSessionFinishedEvent : ModelEvent
    {
        public PracticeSessionFinishedEvent(string id, PracticeWordResult[] wordResults)
            : base(id)
        {
            WordResults = wordResults;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private PracticeSessionFinishedEvent()
#pragma warning restore 612, 618
        {
        }

        public PracticeWordResult[] WordResults { get; private set; }

        public class PracticeWordResult
        {
            public PracticeWordResult(PracticeWord practiceWord)
            {
                if (practiceWord == null) throw new ArgumentNullException("practiceWord");
                WordId = practiceWord.WordId;
                OwnerId = practiceWord.OwnerId;
                ConfidenceLevels = practiceWord.ConfidenceLevels;
            }

            public string WordId { get; private set; }

            public string OwnerId { get; private set; }

            public ConfidenceLevel[] ConfidenceLevels { get; private set; }
        }
    }
}