using System;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordConfidenceUpdated : ModelEvent
    {
        public WordConfidenceUpdated(string wordId, string wordListId, ConfidenceLevel confidenceLevel, string ownerId)
            : base(wordId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");

            WordId = wordId;
            WordListId = wordListId;
            ConfidenceLevel = confidenceLevel;
            OwnerId = ownerId;
        }

        [JsonConstructor]
        private WordConfidenceUpdated()
        {
        }

        public string WordId { get; private set; }

        public string WordListId { get; private set; }

        public ConfidenceLevel ConfidenceLevel { get; private set; }

        public string OwnerId { get; private set; }
    }
}