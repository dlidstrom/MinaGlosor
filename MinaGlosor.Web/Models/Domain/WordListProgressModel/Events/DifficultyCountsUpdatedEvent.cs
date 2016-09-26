using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Domain.WordListProgressModel.Events
{
    public class DifficultyCountsUpdatedEvent : ModelEvent
    {
        public DifficultyCountsUpdatedEvent(
            string modelId,
            int newNumberOfEasyWords,
            int newPercentEasyWords,
            int newNumberOfDifficultWords,
            int newPercentDifficultWords)
            : base(modelId)
        {
            NewNumberOfEasyWords = newNumberOfEasyWords;
            NewPercentEasyWords = newPercentEasyWords;
            NewNumberOfDifficultWords = newNumberOfDifficultWords;
            NewPercentDifficultWords = newPercentDifficultWords;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private DifficultyCountsUpdatedEvent()
#pragma warning restore 612, 618
        {
        }

        public int NewNumberOfEasyWords { get; private set; }

        public int NewPercentEasyWords { get; private set; }
        
        public int NewNumberOfDifficultWords { get; private set; }

        public int NewPercentDifficultWords { get; private set; }
    }
}