using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class ToggleWordFavouriteCommand : ICommand<ToggleWordFavouriteCommand.Result>
    {
        public ToggleWordFavouriteCommand(string wordId, bool isFavourite, string userId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (userId == null) throw new ArgumentNullException("userId");

            WordId = Word.ToId(wordId);
            IsFavourite = isFavourite;
            UserId = userId;
        }

        public string WordId { get; private set; }

        public bool IsFavourite { get; private set; }

        public string UserId { get; private set; }

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