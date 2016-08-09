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
                string id,
                string ownerId,
                string wordListId)
                : base(id)
            {
                if (ownerId == null) throw new ArgumentNullException("ownerId");
                if (wordListId == null) throw new ArgumentNullException("wordListId");
                Apply(new CeatedEvent(id, ownerId, wordListId));
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

            public int NumberOfWordsCurrentlyDone { get; private set; }

            public int PercentDone { get; private set; }

            public int NumberOfWordsExpired { get; private set; }

            public int PercentExpired { get; private set; }

            // todo make sure this gets called
            public void WordHasExpired(int numberOfWords)
            {
                Apply(new WordHasExpiredEvent(Id, NumberOfWordsExpired + 1, CalculatePercentExpired(numberOfWords)));
            }

            // todo make sure this gets called
            public void WordIsUpToDate(int numberOfWords)
            {
                //NumberOfWordsExpired--;
                //PercentExpired = CalculatePercentExpired(numberOfWords);
            }

            // todo make sure this gets called
            public void NewWordHasBeenPracticed(int numberOfWords)
            {
                //NumberOfWordScores++;
                //PercentDone = (int)Math.Floor(100.0 * NumberOfWordScores / Math.Max(1, numberOfWords));
            }

            private int CalculatePercentExpired(int numberOfWords)
            {
                var percentExpired = (int)Math.Floor(100.0 * NumberOfWordsExpired / Math.Max(1, numberOfWords));
                return percentExpired;
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
    }
}