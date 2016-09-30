using System;
using JetBrains.Annotations;
using MinaGlosor.Web.Models.Domain.WordListProgressModel.Events;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Domain.WordListProgressModel
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
            Apply(new CeatedEvent(Id, ownerId, wordListId));
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private Progress()
#pragma warning restore 612, 618
        {
        }

        public string OwnerId { get; private set; }

        public string WordListId { get; private set; }

        public int NumberOfWordScores { get; private set; }

        public int PercentDone { get; private set; }

        public int NumberOfWordsExpired { get; private set; }

        public int PercentExpired { get; private set; }

        public int NumberOfEasyWords { get; private set; }

        public int PercentEasyWords { get; private set; }

        public int NumberOfDifficultWords { get; private set; }

        public int PercentDifficultWords { get; private set; }

        public static string GetIdFromWordListForUser(string wordListId, string ownerId)
        {
            var id = string.Format("WordListProgress-{0}-{1}", User.FromId(ownerId), WordList.FromId(wordListId));
            return id;
        }

        public void WordHasExpired(int numberOfWords)
        {
            var newNumberOfWordsExpired = NumberOfWordsExpired + 1;
            var percentExpired = CalculatePercent(newNumberOfWordsExpired, numberOfWords);
            Apply(new WordHasExpiredEvent(Id, newNumberOfWordsExpired, percentExpired));
        }

        public void WordIsUpToDate(int numberOfWords)
        {
            var newNumberOfWordsExpired = NumberOfWordsExpired - 1;
            var percentExpired = CalculatePercent(newNumberOfWordsExpired, numberOfWords);
            Apply(new WordIsUpToDateEvent(Id, newNumberOfWordsExpired, percentExpired));
        }

        public void NewWordHasBeenPracticed(int numberOfWords)
        {
            var newNumberOfWordScores = NumberOfWordScores + 1;
            var percentDone = CalculatePercent(newNumberOfWordScores, numberOfWords);
            var newPercentEasyWords = CalculatePercent(NumberOfEasyWords, numberOfWords);
            var newPercentDifficultWords = CalculatePercent(NumberOfDifficultWords, numberOfWords);
            Apply(new WordHasBeenPracticedEvent(Id, newNumberOfWordScores, percentDone, newPercentEasyWords, newPercentDifficultWords));
        }

        public void UpdateDifficultyCounts(WordScoreDifficultyLifecycle lifecycle)
        {   
            var newNumberOfEasyWords = NumberOfEasyWords;
            var newNumberOfDifficultWords = NumberOfDifficultWords;
            switch (lifecycle)
            {
                case WordScoreDifficultyLifecycle.FirstTimeDifficult:
                {
                    newNumberOfDifficultWords++;
                    break;
                }

                case WordScoreDifficultyLifecycle.TurnedDifficult:
                {
                    newNumberOfDifficultWords++;
                    newNumberOfEasyWords--;
                    break;
                }

                case WordScoreDifficultyLifecycle.FirstTimeEasy:
                {
                    newNumberOfEasyWords++;
                    break;
                }

                case WordScoreDifficultyLifecycle.TurnedEasy:
                {
                    newNumberOfEasyWords++;
                    newNumberOfDifficultWords--;
                    break;
                }
            }

            var newPercentEasyWords = CalculatePercent(newNumberOfEasyWords, NumberOfWordScores);
            var newPercentDifficultWords = CalculatePercent(newNumberOfDifficultWords, NumberOfWordScores);
            var @event = new DifficultyCountsUpdatedEvent(
                Id,
                newNumberOfEasyWords,
                newPercentEasyWords,
                newNumberOfDifficultWords,
                newPercentDifficultWords);
            Apply(@event);
        }

        private static int CalculatePercent(int wordScores, int numberOfWords)
        {
            var percentDone = (int)Math.Floor(100.0 * wordScores / Math.Max(1, numberOfWords));
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
            PercentEasyWords = @event.NewPercentEasyWords;
            PercentDifficultWords = @event.NewPercentDifficultWords;
        }

        private void ApplyEvent(DifficultyCountsUpdatedEvent @event)
        {
            NumberOfEasyWords = @event.NewNumberOfEasyWords;
            PercentEasyWords = @event.NewPercentEasyWords;
            NumberOfDifficultWords = @event.NewNumberOfDifficultWords;
            PercentDifficultWords = @event.NewPercentDifficultWords;
        }
    }
}