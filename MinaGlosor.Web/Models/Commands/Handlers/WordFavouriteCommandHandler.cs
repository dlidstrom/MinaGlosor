using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class WordFavouriteCommandHandler : ICommandHandler<ToggleWordFavouriteCommand, ToggleWordFavouriteCommand.Result>
    {
        public IDocumentSession Session { get; set; }

        public ToggleWordFavouriteCommand.Result Handle(ToggleWordFavouriteCommand command)
        {
            var id = WordFavourite.GetId(command.WordId, command.UserId);
            var wordFavourite = Session.Load<WordFavourite>(id);
            if (wordFavourite == null)
            {
                // is favourite by default
                wordFavourite = new WordFavourite(command.WordId, command.UserId);
                Session.Store(wordFavourite);
            }
            else
            {
                wordFavourite.Toggle(command.IsFavourite);
            }

            return new ToggleWordFavouriteCommand.Result(wordFavourite.IsFavourite);
        }

        public bool CanExecute(ToggleWordFavouriteCommand command, User currentUser)
        {
            return command.UserId == currentUser.Id;
        }
    }
}