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

        [JsonConstructor]
        private WordScoreRegisteredEvent()
        {
        }

        public string OwnerId { get; private set; }

        public string WordId { get; private set; }

        public string WordListId { get; private set; }

        public double Score { get; private set; }
    }
}