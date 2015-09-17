using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class ResetWordScoreCommand : ICommand<object>
    {
        public ResetWordScoreCommand(string wordScoreId)
        {
            WordScoreId = wordScoreId;
        }

        public string WordScoreId { get; private set; }
    }
}