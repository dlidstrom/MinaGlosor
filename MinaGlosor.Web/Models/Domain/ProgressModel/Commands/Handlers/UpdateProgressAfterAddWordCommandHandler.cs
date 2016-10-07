using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Domain.ProgressModel.Commands.Handlers
{
    public class UpdateProgressAfterAddWordCommandHandler : CommandHandlerBase<UpdateProgressAfterAddWordCommand, object>
    {
        public override object Handle(UpdateProgressAfterAddWordCommand command)
        {
            var progress = Session.Load<Progress>(command.ProgressId);
            progress.WordAdded(command.NumberOfWords);
            return new object();
        }

        public override bool CanExecute(UpdateProgressAfterAddWordCommand command, User currentUser)
        {
            return true;
        }
    }
}