using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class CreateWordListCommandHandler : CommandHandlerBase<CreateWordListCommand, CreateWordListCommand.Result>
    {
        public override CreateWordListCommand.Result Handle(CreateWordListCommand command)
        {
            var id = KeyGeneratorBase.Generate<WordList>(Session);
            var wordList = new WordList(id, command.Name, command.OwnerId);
            Session.Store(wordList);
            return new CreateWordListCommand.Result(wordList);
        }

        public override bool CanExecute(CreateWordListCommand command, User currentUser)
        {
            return true;
        }
    }
}