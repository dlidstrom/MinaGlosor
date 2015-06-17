using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordScoreRegisteredEvent : ModelEvent
    {
        public WordScoreRegisteredEvent(string id, string ownerId, string wordId, string wordListId, double score)
            : base(id)
        {
            OwnerId = ownerId;
            WordId = wordId;
            WordListId = wordListId;
            Score = score;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private WordScoreRegisteredEvent()
#pragma warning restore 612, 618
        {
        }

        public string OwnerId { get; private set; }

        public string WordId { get; private set; }

        public string WordListId { get; private set; }

        public double Score { get; private set; }
    }
}