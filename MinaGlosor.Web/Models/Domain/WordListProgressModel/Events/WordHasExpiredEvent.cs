using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Domain.WordListProgressModel.Events
{
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