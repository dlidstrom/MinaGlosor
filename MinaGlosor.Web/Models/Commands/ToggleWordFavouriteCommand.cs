using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class ToggleWordFavouriteCommand : ICommand<ToggleWordFavouriteCommand.Result>
    {
        private readonly string wordId;
        private readonly bool isFavourite;
        private readonly string userId;

        public ToggleWordFavouriteCommand(string wordId, bool isFavourite, string userId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (userId == null) throw new ArgumentNullException("userId");

            this.wordId = Word.ToId(wordId);
            this.isFavourite = isFavourite;
            this.userId = userId;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return userId == currentUser.Id;
        }

        public Result Execute(IDocumentSession session)
        {
            var id = WordFavourite.GetId(wordId, userId);
            var wordFavourite = session.Load<WordFavourite>(id);
            if (wordFavourite == null)
            {
                // is favourite by default
                wordFavourite = new WordFavourite(wordId, userId);
                session.Store(wordFavourite);
            }
            else
            {
                wordFavourite.Toggle(isFavourite);
            }

            return new Result(wordFavourite.IsFavourite);
        }

        public class Result
        {
            public Result(bool isFavourite)
            {
                IsFavourite = isFavourite;
            }

            public bool IsFavourite { get; private set; }
        }
    }
}