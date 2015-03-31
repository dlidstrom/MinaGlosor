using System;
using System.Linq;
using Raven.Abstractions;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class PracticeSession
    {
        public PracticeSession(string wordListId, PracticeWord[] words, string ownerId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (words == null) throw new ArgumentNullException("words");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (words.Length == 0) throw new ArgumentException("No words to practice", "words");

            WordListId = wordListId;
            Words = words;
            OwnerId = ownerId;
            CreatedDate = SystemTime.UtcNow;
        }

        [JsonConstructor]
        private PracticeSession()
        {
        }

        public string Id { get; private set; }

        public string WordListId { get; private set; }

        public PracticeWord[] Words { get; private set; }

        public string OwnerId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public bool IsFinished
        {
            get { return Words.Any(x => x.Confidence < (int)ConfidenceLevel.CorrectAfterHesitation) == false; }
        }

        private int NumberOfWordsEasilyLearnt
        {
            get { return Words.Count(x => x.Confidence >= (int)ConfidenceLevel.CorrectAfterHesitation); }
        }

        private int NumberOfWordsRecalledWithDifficulty
        {
            get { return Words.Count(x => x.Confidence == (int)ConfidenceLevel.RecalledWithSeriousDifficulty); }
        }

        private int NumberOfWordsForgotten
        {
            get { return Words.Count(x => x.Confidence < (int)ConfidenceLevel.RecalledWithSeriousDifficulty && x.Confidence > (int)ConfidenceLevel.CompleteBlackout); }
        }

        public static string ToId(string practiceSessionId)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");

            return "PracticeSessions/" + practiceSessionId;
        }

        public static string FromId(string practiceSessionId)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");

            return practiceSessionId.Substring(17);
        }

        public PracticeSessionStatistics GetStatistics()
        {
            return new PracticeSessionStatistics(
                IsFinished,
                Words.Length,
                NumberOfWordsEasilyLearnt,
                NumberOfWordsRecalledWithDifficulty,
                NumberOfWordsForgotten);
        }

        public PracticeWord GetNextWord()
        {
            if (IsFinished)
                throw new ApplicationException("Practice session is finishe");

            var firstUnscored = Words.FirstOrDefault(x => x.Confidence < 0);
            if (firstUnscored != null)
            {
                firstUnscored.UpdateLastPickedDate();
                return firstUnscored;
            }

            var nextWord = Words.Where(x => x.Confidence < 4).OrderBy(x => x.LastPickedDate).FirstOrDefault();
            if (nextWord == null) throw new ApplicationException("No more words to practice");
            nextWord.UpdateLastPickedDate();
            return nextWord;
        }

        public void UpdateConfidence(string practiceWordId, ConfidenceLevel confidenceLevel)
        {
            if (practiceWordId == null) throw new ArgumentNullException("practiceWordId");

            var practiceWord = Words.Single(x => x.PracticeWordId == practiceWordId);
            practiceWord.UpdateConfidence(confidenceLevel);
        }

        public bool HasAccess(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");

            return OwnerId == userId;
        }

        public PracticeWord GetWordById(string practiceWordId)
        {
            var practiceWord = Words.Single(x => x.PracticeWordId == practiceWordId);
            return practiceWord;
        }
    }
}