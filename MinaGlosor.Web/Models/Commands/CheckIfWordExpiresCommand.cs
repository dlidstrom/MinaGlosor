using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class CheckIfWordExpiresCommand : ICommand<object>
    {
        public CheckIfWordExpiresCommand(string wordScoreId)
        {
            WordScoreId = wordScoreId;
        }

        public string WordScoreId { get; private set; }
    }
}