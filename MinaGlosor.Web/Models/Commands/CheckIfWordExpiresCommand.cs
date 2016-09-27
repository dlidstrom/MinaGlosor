using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class CheckIfWordExpiresCommand : ICommand<object>
    {
        public CheckIfWordExpiresCommand(string wordScoreId, WordDifficultyUpdate wordDifficultyUpdate)
        {
            WordScoreId = wordScoreId;
            WordDifficultyUpdate = wordDifficultyUpdate;
        }

        public string WordScoreId { get; private set; }
        public WordDifficultyUpdate WordDifficultyUpdate { get; private set; }
    }
}