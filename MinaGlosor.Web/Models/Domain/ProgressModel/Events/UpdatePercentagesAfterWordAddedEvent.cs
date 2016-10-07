using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Domain.ProgressModel.Events
{
    public class UpdatePercentagesAfterWordAddedEvent : ModelEvent
    {
        public UpdatePercentagesAfterWordAddedEvent(
            string modelId,
            ProgressPercentages progressPercentages)
            : base(modelId)
        {
            ProgressPercentages = progressPercentages;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private UpdatePercentagesAfterWordAddedEvent()
#pragma warning restore 612, 618
        {
        }

        public ProgressPercentages ProgressPercentages { get; private set; }
    }
}