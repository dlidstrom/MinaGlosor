using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class CreateWordCommandHandler : CommandHandlerBase<CreateWordCommand, string>
    {
        public override string Handle(CreateWordCommand command)
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

        public override bool CanExecute(CreateWordCommand command, User currentUser)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            var hasAccess = wordList.HasAccess(currentUser.Id);
            return hasAccess;
        }
    }
}