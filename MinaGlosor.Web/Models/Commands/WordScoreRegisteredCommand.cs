using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class WordScoreRegisteredCommand : ICommand<object>
    {
        public WordScoreRegisteredCommand(string wordId, string ownerId)
        {
            WordId = wordId;
            OwnerId = ownerId;
        }

        public string WordId { get; private set; }
        public string OwnerId { get; private set; }
    }
}