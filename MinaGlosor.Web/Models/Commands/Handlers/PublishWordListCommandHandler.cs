using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class PublishWordListCommandHandler : CommandHandlerBase<PublishWorListCommand, object>
    {
        public override object Handle(PublishWorListCommand command)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            if (command.Publish) wordList.Publish();
            else wordList.Unpublish();
            return new object();
        }

        public override bool CanExecute(PublishWorListCommand command, User currentUser)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            var canExecute = wordList.OwnerId == currentUser.Id;
            return canExecute;
        }
    }
}