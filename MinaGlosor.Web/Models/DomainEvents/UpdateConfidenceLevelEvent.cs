using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class UpdateConfidenceLevelEvent : ModelEvent
    {
        public UpdateConfidenceLevelEvent(
            string id,
            string practiceWordId,
            ConfidenceLevel confidenceLevel,
            string wordId,
            string wordListId,
            string ownerId)
            : base(id)
        {
            PracticeWordId = practiceWordId;
            ConfidenceLevel = confidenceLevel;
            WordId = wordId;
            WordListId = wordListId;
            OwnerId = ownerId;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private UpdateConfidenceLevelEvent()
#pragma warning restore 612, 618
        {
        }

        public string PracticeWordId { get; private set; }

        public ConfidenceLevel ConfidenceLevel { get; private set; }

        public string WordId { get; private set; }

        public string WordListId { get; private set; }

        public string OwnerId { get; private set; }
    }
}