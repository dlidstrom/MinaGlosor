using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class ScoreWordCommand : ICommand<object>
    {
        public ScoreWordCommand(
            string ownerId,
            string wordId,
            string wordListId,
            ConfidenceLevel confidenceLevel)
        {
            OwnerId = ownerId;
            WordId = wordId;
            WordListId = wordListId;
            ConfidenceLevel = confidenceLevel;
        }

        public string OwnerId { get; private set; }

        public string WordId { get; private set; }

        public string WordListId { get; private set; }

        public ConfidenceLevel ConfidenceLevel { get; private set; }
    }
}