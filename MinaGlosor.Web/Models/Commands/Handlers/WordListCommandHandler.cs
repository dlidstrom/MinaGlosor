using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class WordListCommandHandler :
        ICommandHandler<AddWordToWordListCommand, object>,
        ICommandHandler<CreateWordListCommand, CreateWordListCommand.Result>,
        ICommandHandler<UpdateWordListNameCommand, object>,
        ICommandHandler<PublishWordListCommand, object>
    {
        public IDocumentSession Session { get; set; }

        public object Handle(AddWordToWordListCommand command)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            wordList.AddWord();
            return null;
        }

        public bool CanExecute(AddWordToWordListCommand command, User currentUser)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            var hasAccess = wordList.HasAccess(currentUser.Id);
            return hasAccess;
        }

        public CreateWordListCommand.Result Handle(CreateWordListCommand command)
        {
            var id = KeyGeneratorBase.Generate<WordList>(Session);
            var wordList = new WordList(id, command.Name, command.OwnerId);
            Session.Store(wordList);
            return new CreateWordListCommand.Result(wordList);
        }

        public bool CanExecute(CreateWordListCommand command, User currentUser)
        {
            return true;
        }

        public object Handle(UpdateWordListNameCommand command)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            wordList.UpdateName(command.WordListName);
            return new object();
        }

        public bool CanExecute(UpdateWordListNameCommand command, User currentUser)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            var canExecute = wordList.OwnerId == currentUser.Id;
            return canExecute;
        }

        public object Handle(PublishWordListCommand command)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            if (command.Publish) wordList.Publish();
            else wordList.Unpublish();
            return new object();
        }

        public bool CanExecute(PublishWordListCommand command, User currentUser)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            var canExecute = wordList.OwnerId == currentUser.Id;
            return canExecute;
        }
    }
}