using System;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Abstractions;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class PracticeWord
    {
        public PracticeWord(Word word, string wordListId, string ownerId)
        {
            if (word == null) throw new ArgumentNullException("word");
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");

            WordId = word.Id;
            WordListId = wordListId;
            CreatedDate = word.CreatedDate;
            Confidence = -1;
            PracticeWordId = Guid.NewGuid().ToString("N");
            OwnerId = ownerId;
        }

        [JsonConstructor]
        private PracticeWord()
        {
        }

        public string WordListId { get; private set; }

        public string WordId { get; private set; }

        public int Confidence { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime? LastPickedDate { get; private set; }

        public string PracticeWordId { get; private set; }

        public string OwnerId { get; private set; }

        public void UpdateConfidence(ConfidenceLevel confidenceLevel)
        {
            if (confidenceLevel < ConfidenceLevel.CompleteBlackout || confidenceLevel > ConfidenceLevel.PerfectResponse)
                throw new ArgumentException("Unknown confidence level", "confidenceLevel");

            Confidence = (int)confidenceLevel;
            DomainEvent.Raise(new WordConfidenceUpdated(WordId, WordListId, confidenceLevel, OwnerId));
        }

        public void UpdateLastPickedDate()
        {
            LastPickedDate = SystemTime.UtcNow;
        }
    }
}