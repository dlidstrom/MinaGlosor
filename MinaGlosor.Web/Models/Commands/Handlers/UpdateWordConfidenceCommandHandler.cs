using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class UpdateWordConfidenceCommandHandler : CommandHandlerBase<UpdateWordConfidenceCommand, UpdateWordConfidenceCommand.Result>
    {
        public override UpdateWordConfidenceCommand.Result Handle(UpdateWordConfidenceCommand command)
        {
            var practiceSession = Session.Load<PracticeSession>(command.PracticeSessionId);
            practiceSession.UpdateConfidence(command.PracticeWordId, command.ConfidenceLevel);
            return new UpdateWordConfidenceCommand.Result(practiceSession.GetStatistics());
        }

        public override bool CanExecute(UpdateWordConfidenceCommand command, User currentUser)
        {
            var practiceSession = Session.Load<PracticeSession>(command.PracticeSessionId);
            var hasAccess = practiceSession.HasAccess(currentUser.Id);
            return hasAccess;
        }
    }
}