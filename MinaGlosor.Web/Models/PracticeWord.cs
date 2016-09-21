using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class PracticeWord
    {
        [JsonProperty("ConfidenceLevels")]
        private readonly List<ConfidenceLevel> confidenceLevels = new List<ConfidenceLevel>();
 
        public PracticeWord(Word word, string wordListId, string ownerId)
        {
            if (word == null) throw new ArgumentNullException("word");
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");

            WordId = word.Id;
            WordListId = wordListId;
            CreatedDate = word.CreatedDate;
            Confidence = (int)ConfidenceLevel.Unknown;
            PracticeWordId = Guid.NewGuid().ToString("N").Substring(0, 7);
            OwnerId = ownerId;
        }

        [JsonConstructor]
        private PracticeWord(IEnumerable<ConfidenceLevel> confidenceLevels)
        {
            this.confidenceLevels = confidenceLevels.ToList();
        }

        public string WordListId { get; private set; }

        public string WordId { get; private set; }

        public int Confidence { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime? LastPickedDate { get; private set; }

        public string PracticeWordId { get; private set; }

        public string OwnerId { get; private set; }

        [JsonIgnore]
        public ConfidenceLevel[] ConfidenceLevels
        {
            get
            {
                return confidenceLevels.ToArray();
            }
        }

        public void UpdateConfidence(ConfidenceLevel confidenceLevel)
        {
            if (confidenceLevel < ConfidenceLevel.CompleteBlackout || confidenceLevel > ConfidenceLevel.PerfectResponse)
                throw new ArgumentException("Unknown confidence level", "confidenceLevel");

            if (Confidence >= (int)ConfidenceLevel.CorrectAfterHesitation)
                throw new ApplicationException("Cannot score word twice");

            Confidence = (int)confidenceLevel;
            confidenceLevels.Add(confidenceLevel);
        }

        public void UpdateLastPickedDate(DateTime date)
        {
            LastPickedDate = date;
        }
    }
}