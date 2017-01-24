using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Domain.ProgressModel.Events
{
    public class CeatedEvent : ModelEvent
    {
        public CeatedEvent(
            string modelId,
            string ownerId,
            string wordListId,
            ProgressWordCounts progressWordCounts,
            ProgressPercentages progressPercentages,
            ProgressSortOrder progressSortOrder)
            : base(modelId)
        {
            OwnerId = ownerId;
            WordListId = wordListId;
            ProgressWordCounts = progressWordCounts;
            ProgressPercentages = progressPercentages;
            ProgressSortOrder = progressSortOrder;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private CeatedEvent()
#pragma warning restore 612, 618
        {
        }

        public string OwnerId { get; private set; }

        public string WordListId { get; private set; }

        public ProgressWordCounts ProgressWordCounts { get; private set; }

        public ProgressPercentages ProgressPercentages { get; private set; }

        public ProgressSortOrder ProgressSortOrder { get; private set; }
    }
}