using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class ResetWordScoreCommand : ICommand<object>
    {
        private readonly string wordScoreId;

        public ResetWordScoreCommand(string wordScoreId)
        {
            this.wordScoreId = wordScoreId;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public void Execute(IDocumentSession session)
        {
            var wordScore = session.Load<WordScore>(wordScoreId);
            wordScore.ResetAfterWordEdit();
        }
    }
}