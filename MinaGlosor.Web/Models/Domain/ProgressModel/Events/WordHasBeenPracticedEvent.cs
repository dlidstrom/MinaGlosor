using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Domain.ProgressModel.Events
{
    public class WordHasBeenPracticedEvent : ModelEvent
    {
        public WordHasBeenPracticedEvent(
            string modelId,
            ProgressWordCounts progressWordCounts,
            ProgressPercentages progressPercentages)
            : base(modelId)
        {
            ProgressWordCounts = progressWordCounts;
            ProgressPercentages = progressPercentages;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private WordHasBeenPracticedEvent()
#pragma warning restore 612, 618
        {
        }

        public ProgressWordCounts ProgressWordCounts { get; private set; }

        public ProgressPercentages ProgressPercentages { get; private set; }
    }
}