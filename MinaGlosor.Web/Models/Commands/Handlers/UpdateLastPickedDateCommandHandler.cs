using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class UpdateLastPickedDateCommandHandler : CommandHandlerBase<UpdateLastPickedDateCommand, object>
    {
        public override object Handle(UpdateLastPickedDateCommand command)
        {
            var practiceSession = Session.Load<PracticeSession>(command.PracticeSessionId);
            practiceSession.UpdateLastPickedDate(command.PracticeWordId);
            return null;
        }

        public override bool CanExecute(UpdateLastPickedDateCommand command, User currentUser)
        {
            return true;
        }
    }
}