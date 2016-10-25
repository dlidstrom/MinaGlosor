﻿using System;
using System.Linq;
using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Abstractions;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class PracticeSession : DomainModel
    {
        public PracticeSession(string id, string wordListId, PracticeWord[] words, string ownerId)
            : base(id)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (words == null) throw new ArgumentNullException("words");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (words.Length == 0) throw new ArgumentException("No words to practice", "words");

            Apply(new PracticeSessionCreatedEvent(id, wordListId, words, ownerId));
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private PracticeSession()
#pragma warning restore 612, 618
        {
        }

        public string WordListId { get; private set; }

        public PracticeWord[] Words { get; private set; }

        public string OwnerId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public bool IsFinished { get; private set; }

        public string CurrentPracticeWordId { get; private set; }

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

        public bool HasAccess(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");

            return OwnerId == userId;
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
                throw new ApplicationException("Practice session is finished");

            PracticeWord nextWord;
            var firstUnscored = Words.FirstOrDefault(x => x.Confidence < 0);
            if (firstUnscored != null)
            {
                nextWord = firstUnscored;
            }
            else
            {

                var oldestWord = Words.Where(x => x.Confidence < 4).OrderBy(x => x.LastPickedDate).FirstOrDefault();
                if (oldestWord == null) throw new ApplicationException("No more words to practice");
                nextWord = oldestWord;
            }

            return nextWord;
        }

        public void UpdateLastPickedDate(string practiceWordId)
        {
            if (practiceWordId == null) throw new ArgumentNullException("practiceWordId");
            if (CurrentPracticeWordId != practiceWordId)
            {
                Apply(new UpdateLastPickedDateEvent(Id, practiceWordId, SystemTime.UtcNow));
            }
        }

        public void UpdateConfidence(string practiceWordId, ConfidenceLevel confidenceLevel)
        {
            if (practiceWordId == null) throw new ArgumentNullException("practiceWordId");
            var practiceWord = Words.Single(x => x.PracticeWordId == practiceWordId);
            Apply(new UpdateConfidenceLevelEvent(Id, practiceWordId, confidenceLevel, practiceWord.WordId, practiceWord.WordListId, OwnerId));
            if (Words.All(x => x.Confidence >= (int)ConfidenceLevel.CorrectAfterHesitation))
            {
                var wordResults = Words.Select(x => new PracticeSessionFinishedEvent.PracticeWordResult(x)).ToArray();
                Apply(new PracticeSessionFinishedEvent(Id, wordResults));
            }
        }

        public PracticeWord GetWordById(string practiceWordId)
        {
            if (practiceWordId == null) throw new ArgumentNullException("practiceWordId");
            var practiceWord = Words.Single(x => x.PracticeWordId == practiceWordId);
            return practiceWord;
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

        private void ApplyEvent(UpdateConfidenceLevelEvent @event)
        {
            var practiceWord = Words.Single(x => x.PracticeWordId == @event.PracticeWordId);
            practiceWord.UpdateConfidence(@event.ConfidenceLevel);
        }

        private void ApplyEvent(UpdateLastPickedDateEvent @event)
        {
            var nextWord = Words.Single(x => x.PracticeWordId == @event.PracticeWordId);
            nextWord.UpdateLastPickedDate(@event.Date);
            CurrentPracticeWordId = nextWord.PracticeWordId;
        }

        private void ApplyEvent(PracticeSessionCreatedEvent @event)
        {
            WordListId = @event.WordListId;
            Words = @event.Words;
            OwnerId = @event.OwnerId;
            CreatedDate = @event.CreatedDateTime;
        }

        private void ApplyEvent(PracticeSessionFinishedEvent @event)
        {
            IsFinished = true;
        }
    }
}