using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Domain.ProgressModel.Events
{
    public class UpdatePercentagesAfterWordAddedEvent : ModelEvent
    {
        public UpdatePercentagesAfterWordAddedEvent(
            string modelId,
            ProgressPercentages progressPercentages,
            ProgressSortOrder progressSortOrder)
            : base(modelId)
        {
            ProgressPercentages = progressPercentages;
            ProgressSortOrder = progressSortOrder;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private UpdatePercentagesAfterWordAddedEvent()
#pragma warning restore 612, 618
        {
        }

        public ProgressPercentages ProgressPercentages { get; private set; }

        public ProgressSortOrder ProgressSortOrder { get; private set; }
    }
}