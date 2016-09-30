using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class CheckIfWordExpiresCommandHandler : CommandHandlerBase<CheckIfWordExpiresCommand, object>
    {
        public override object Handle(CheckIfWordExpiresCommand command)
        {
            var wordScore = Session.Load<WordScore>(command.WordScoreId);
            wordScore.CheckIfWordExpires();
            return new object();
        }

        public override bool CanExecute(CheckIfWordExpiresCommand command, User currentUser)
        {
            return true;
        }
    }
}