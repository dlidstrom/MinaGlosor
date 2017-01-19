using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class WordCommandHandler :
        ICommandHandler<CreateWordCommand, string>,
        ICommandHandler<UpdateWordCommand, object>
    {
        public IDocumentSession Session { get; set; }

        public string Handle(CreateWordCommand command)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            var word = Word.Create(
                KeyGeneratorBase.Generate<Word>(Session),
                command.Text,
                command.Definition,
                wordList);
            Session.Store(word);
            return Word.FromId(word.Id);
        }

        public bool CanExecute(CreateWordCommand command, User currentUser)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            var hasAccess = wordList.HasAccess(currentUser.Id);
            return hasAccess;
        }

        public object Handle(UpdateWordCommand command)
        {
            var word = Session.Load<Word>(command.WordId);
            word.Update(command.Text, command.Definition);
            return null;
        }

        public bool CanExecute(UpdateWordCommand command, User currentUser)
        {
            var word = Session.Load<Word>(command.WordId);
            var wordList = Session.Load<WordList>(word.WordListId);
            var hasAccess = wordList.HasAccess(currentUser.Id);
            return hasAccess;
        }
    }
}