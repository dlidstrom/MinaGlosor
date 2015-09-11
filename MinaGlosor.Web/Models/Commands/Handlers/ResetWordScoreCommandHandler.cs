using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class ResetWordScoreCommandHandler : CommandHandlerBase<ResetWordScoreCommand, object>
    {
        public override object Handle(ResetWordScoreCommand command)
        {
            var wordScore = Session.Load<WordScore>(command.WordScoreId);
            wordScore.ResetAfterWordEdit();
            return null;
        }

        public override bool CanExecute(ResetWordScoreCommand command, User currentUser)
        {
            return true;
        }
    }
}