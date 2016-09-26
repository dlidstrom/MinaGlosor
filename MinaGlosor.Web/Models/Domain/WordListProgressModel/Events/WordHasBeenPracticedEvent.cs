using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Domain.WordListProgressModel.Events
{
    public class WordHasBeenPracticedEvent : ModelEvent
    {
        public WordHasBeenPracticedEvent(
            string modelId,
            int newNumberOfWordScores,
            int percentDone,
            int newPercentEasyWords,
            int newPercentDifficultWords)
            : base(modelId)
        {
            NewNumberOfWordScores = newNumberOfWordScores;
            PercentDone = percentDone;
            NewPercentEasyWords = newPercentEasyWords;
            NewPercentDifficultWords = newPercentDifficultWords;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private WordHasBeenPracticedEvent()
#pragma warning restore 612, 618
        {
        }

        public int NewNumberOfWordScores { get; private set; }

        public int PercentDone { get; private set; }

        public int NewPercentEasyWords { get; private set; }

        public int NewPercentDifficultWords { get; private set; }
    }
}