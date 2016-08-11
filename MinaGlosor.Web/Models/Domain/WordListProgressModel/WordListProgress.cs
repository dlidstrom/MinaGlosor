using System;
using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Domain.WordListProgressModel
{
    public static class WordListProgress
    {
        public class Model : DomainModel
        {
            public Model(
                string ownerId,
                string wordListId)
                : base(GetIdFromWordListForUser(wordListId, ownerId))
            {
                if (ownerId == null) throw new ArgumentNullException("ownerId");
                if (wordListId == null) throw new ArgumentNullException("wordListId");
                Apply(new CeatedEvent(Id, ownerId, wordListId));
            }

#pragma warning disable 612, 618
            [JsonConstructor, UsedImplicitly]
            private Model()
#pragma warning restore 612, 618
            {
            }

            public string OwnerId { get; private set; }

            public string WordListId { get; private set; }

            public int NumberOfWordScores { get; private set; }

            public int PercentDone { get; private set; }

            public int NumberOfWordsExpired { get; private set; }

            public int PercentExpired { get; private set; }

            public static string GetIdFromWordListForUser(string wordListId, string ownerId)
            {
                var id = string.Format("WordListProgress-{0}-{1}", User.FromId(ownerId), WordList.FromId(wordListId));
                return id;
            }

            public void WordHasExpired(int numberOfWords)
            {
                var newNumberOfWordsExpired = NumberOfWordsExpired + 1;
                var percentExpired = CalculatePercentExpired(newNumberOfWordsExpired, numberOfWords);
                Apply(new WordHasExpiredEvent(Id, newNumberOfWordsExpired, percentExpired));
            }

            public void WordIsUpToDate(int numberOfWords)
            {
                var newNumberOfWordsExpired = NumberOfWordsExpired - 1;
                var percentExpired = CalculatePercentExpired(newNumberOfWordsExpired, numberOfWords);
                Apply(new WordIsUpToDateEvent(Id, newNumberOfWordsExpired, percentExpired));
            }

            public void NewWordHasBeenPracticed(int numberOfWords)
            {
                var newNumberOfWordScores = NumberOfWordScores + 1;
                var percentDone = CalculatePercentDone(newNumberOfWordScores, numberOfWords);
                Apply(new WordHasBeenPracticedEvent(Id, newNumberOfWordScores, percentDone));
            }

            private static int CalculatePercentExpired(int newNumberOfWordsExpired, int numberOfWords)
            {
                var percentExpired = (int)Math.Floor(100.0 * newNumberOfWordsExpired / Math.Max(1, numberOfWords));
                return percentExpired;
            }

            private static int CalculatePercentDone(int newNumberOfWordScores, int numberOfWords)
            {
                var percentDone = (int)Math.Floor(100.0 * newNumberOfWordScores / Math.Max(1, numberOfWords));
                return percentDone;
            }

            private void ApplyEvent(CeatedEvent @event)
            {
                OwnerId = @event.OwnerId;
                WordListId = @event.WordListId;
            }

            private void ApplyEvent(WordHasExpiredEvent @event)
            {
                NumberOfWordsExpired = @event.NewNumberOfWordsExpired;
                PercentExpired = @event.CalculatedPercentExpired;
            }

            private void ApplyEvent(WordIsUpToDateEvent @event)
            {
                NumberOfWordsExpired = @event.NewNumberOfWordsExpired;
                PercentExpired = @event.PercentExpired;
            }

            private void ApplyEvent(WordHasBeenPracticedEvent @event)
            {
                NumberOfWordScores = @event.NewNumberOfWordScores;
                PercentDone = @event.PercentDone;
            }
        }

        public class CeatedEvent : ModelEvent
        {
            public CeatedEvent(
                string modelId,
                string ownerId,
                string wordListId)
                : base(modelId)
            {
                OwnerId = ownerId;
                WordListId = wordListId;
            }

#pragma warning disable 612, 618
            [JsonConstructor, UsedImplicitly]
            private CeatedEvent()
#pragma warning restore 612, 618
            {
            }

            public string OwnerId { get; private set; }

            public string WordListId { get; private set; }
        }

        public class WordHasExpiredEvent : ModelEvent
        {
            public WordHasExpiredEvent(string modelId, int newNumberOfWordsExpired, int calculatedPercentExpired)
                : base(modelId)
            {
                NewNumberOfWordsExpired = newNumberOfWordsExpired;
                CalculatedPercentExpired = calculatedPercentExpired;
            }

#pragma warning disable 612, 618
            [JsonConstructor, UsedImplicitly]
            private WordHasExpiredEvent()
#pragma warning restore 612, 618
            {
            }

            public int NewNumberOfWordsExpired { get; private set; }

            public int CalculatedPercentExpired { get; private set; }
        }

        public class WordIsUpToDateEvent : ModelEvent
        {
            public WordIsUpToDateEvent(string modelId, int newNumberOfWordsExpired, int percentExpired)
                : base(modelId)
            {
                NewNumberOfWordsExpired = newNumberOfWordsExpired;
                PercentExpired = percentExpired;
            }

#pragma warning disable 612, 618
            [JsonConstructor, UsedImplicitly]
            private WordIsUpToDateEvent()
#pragma warning restore 612, 618
            {
            }

            public int NewNumberOfWordsExpired { get; private set; }

            public int PercentExpired { get; private set; }
        }

        public class WordHasBeenPracticedEvent : ModelEvent
        {
            public WordHasBeenPracticedEvent(string modelId, int newNumberOfWordScores, int percentDone)
                : base(modelId)
            {
                NewNumberOfWordScores = newNumberOfWordScores;
                PercentDone = percentDone;
            }

            #pragma warning disable 612, 618
            [JsonConstructor, UsedImplicitly]
            private WordHasBeenPracticedEvent()
#pragma warning restore 612, 618
            {
            }

            public int NewNumberOfWordScores { get; private set; }

            public int PercentDone { get; private set; }
        }
    }
}