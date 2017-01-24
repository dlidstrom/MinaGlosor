using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Domain.ProgressModel.Events
{
    public class WordHasExpiredEvent : ModelEvent
    {
        public WordHasExpiredEvent(
            string modelId,
            ProgressWordCounts progressWordCounts,
            ProgressPercentages progressPercentages,
            ProgressSortOrder progressSortOrder)
            : base(modelId)
        {
            ProgressWordCounts = progressWordCounts;
            ProgressPercentages = progressPercentages;
            ProgressSortOrder = progressSortOrder;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private WordHasExpiredEvent()
#pragma warning restore 612, 618
        {
        }

        public ProgressWordCounts ProgressWordCounts { get; private set; }

        public ProgressPercentages ProgressPercentages { get; private set; }

        public ProgressSortOrder ProgressSortOrder { get; private set; }
    }
}