using System;
using JetBrains.Annotations;
using MinaGlosor.Web.Models.Domain.ProgressModel.Events;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Domain.ProgressModel
{
    public class Progress : DomainModel
    {
        public Progress(
            string ownerId,
            string wordListId)
            : base(GetIdFromWordListForUser(wordListId, ownerId))
        {
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            var @event = new CeatedEvent(
                Id,
                ownerId,
                wordListId,
                new ProgressWordCounts(),
                new ProgressPercentages(),
                ProgressSortOrder.Default(GetOrder(ownerId, wordListId)));
            Apply(@event);
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private Progress(string id)
#pragma warning restore 612, 618
        {
        }

        public string OwnerId { get; private set; }

        public string WordListId { get; private set; }

        public ProgressWordCounts WordCounts { get; private set; }

        public ProgressPercentages Percentages { get; private set; }

        public ProgressSortOrder SortOrder { get; private set; }

        public static string GetIdFromWordListForUser(string wordListId, string ownerId)
        {
            var id = string.Format("Progress-{0}-{1}", User.FromId(ownerId), WordList.FromId(wordListId));
            return id;
        }

        public static string FromId(string progressId)
        {
            if (progressId == null) throw new ArgumentNullException("progressId");
            return progressId.Substring(9);
        }

        public void WordHasExpired(int numberOfWords)
        {
            var newWordCounts = WordCounts.IncreaseExpired();
            var newPercentages = Percentages.Of(newWordCounts, numberOfWords);
            var newProgressSortOrder = ProgressSortOrder.UpdateWith(GetOrder(OwnerId, WordListId), numberOfWords, newPercentages);
            Apply(new WordHasExpiredEvent(Id, newWordCounts, newPercentages, newProgressSortOrder));
        }

        public void WordIsUpToDate(int numberOfWords)
        {
            var newWordCounts = WordCounts.DecreaseExpired();
            var newPercentages = Percentages.Of(newWordCounts, numberOfWords);
            var newProgressSortOrder = ProgressSortOrder.UpdateWith(GetOrder(OwnerId, WordListId), numberOfWords, newPercentages);
            Apply(new WordIsUpToDateEvent(Id, newWordCounts, newPercentages, newProgressSortOrder));
        }

        public void NewWordHasBeenPracticed(int numberOfWords)
        {
            var newWordCounts = WordCounts.IncreaseCount();
            var newPercentages = Percentages.Of(newWordCounts, numberOfWords);
            var newProgressSortOrder = ProgressSortOrder.UpdateWith(GetOrder(OwnerId, WordListId), numberOfWords, newPercentages);
            Apply(new WordHasBeenPracticedEvent(Id, newWordCounts, newPercentages, newProgressSortOrder));
        }

        public void WordAdded(int numberOfWords)
        {
            var newPercentages = Percentages.Of(WordCounts, numberOfWords);
            var newProgressSortOrder = ProgressSortOrder.UpdateWith(GetOrder(OwnerId, WordListId), numberOfWords, newPercentages);
            Apply(new UpdatePercentagesAfterWordAddedEvent(Id, newPercentages, newProgressSortOrder));
        }

        public void UpdateDifficultyCounts(WordScoreDifficultyLifecycle lifecycle)
        {
            ProgressWordCounts newWordCounts;
            switch (lifecycle)
            {
                case WordScoreDifficultyLifecycle.FirstTimeDifficult:
                {
                    newWordCounts = WordCounts.IncreaseDifficult();
                    break;
                }

                case WordScoreDifficultyLifecycle.TurnedDifficult:
                {
                    newWordCounts = WordCounts.TurnedDifficult();
                    break;
                }

                case WordScoreDifficultyLifecycle.FirstTimeEasy:
                {
                    newWordCounts = WordCounts.IncreaseEasy();
                    break;
                }

                case WordScoreDifficultyLifecycle.TurnedEasy:
                {
                    newWordCounts = WordCounts.TurnedEasy();
                    break;
                }

                case WordScoreDifficultyLifecycle.NoChange:
                {
                    return;
                }

                default: throw new InvalidOperationException("Unknown lifecycle: " + lifecycle);
            }

            var newPercentages = Percentages.Difficulties(newWordCounts);
            var @event = new DifficultyCountsUpdatedEvent(
                Id,
                newWordCounts,
                newPercentages);
            Apply(@event);
        }

        private static int GetOrder(string ownerId, string wordListId)
        {
            var order = 1000000 * int.Parse(User.FromId(ownerId)) + int.Parse(WordList.FromId(wordListId));
            return order;
        }

        private void ApplyEvent(CeatedEvent @event)
        {
            OwnerId = @event.OwnerId;
            WordListId = @event.WordListId;
            WordCounts = @event.ProgressWordCounts;
            Percentages = @event.ProgressPercentages;
            SortOrder = @event.ProgressSortOrder;
        }

        private void ApplyEvent(WordHasExpiredEvent @event)
        {
            WordCounts = @event.ProgressWordCounts;
            Percentages = @event.ProgressPercentages;
            SortOrder = @event.ProgressSortOrder;
        }

        private void ApplyEvent(WordIsUpToDateEvent @event)
        {
            WordCounts = @event.ProgressWordCounts;
            Percentages = @event.ProgressPercentages;
            SortOrder = @event.ProgressSortOrder;
        }

        private void ApplyEvent(WordHasBeenPracticedEvent @event)
        {
            WordCounts = @event.ProgressWordCounts;
            Percentages = @event.ProgressPercentages;
            SortOrder = @event.ProgressSortOrder;
        }

        private void ApplyEvent(DifficultyCountsUpdatedEvent @event)
        {
            WordCounts = @event.ProgressWordCounts;
            Percentages = @event.ProgressPercentages;
        }

        private void ApplyEvent(UpdatePercentagesAfterWordAddedEvent @event)
        {
            Percentages = @event.ProgressPercentages;
            SortOrder = @event.ProgressSortOrder;
        }
    }
}