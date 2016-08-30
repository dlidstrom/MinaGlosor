using System;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetProgressQuery : IQuery<GetProgressQuery.Result>
    {
        public GetProgressQuery(string userId, int page, int itemsPerPage)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            UserId = userId;
            Page = page;
            ItemsPerPage = itemsPerPage;
        }

        public string UserId { get; private set; }

        public int Page { get; private set; }

        public int ItemsPerPage { get; private set; }

        public class Result
        {
            public Result(WordListProgressResult[] wordListProgresses, int numberOfFavourites)
            {
                WordLists = wordListProgresses;
                NumberOfFavourites = numberOfFavourites;
            }

            public WordListProgressResult[] WordLists { get; private set; }

            public int NumberOfFavourites { get; private set; }
        }

        public class WordListProgressResult
        {
            public WordListProgressResult(WordListProgress.Model wordListProgress, WordList wordList)
            {
                WordListId = WordList.FromId(wordListProgress.WordListId);
                OwnerId = User.FromId(wordListProgress.OwnerId);
                Name = wordList.Name;
                NumberOfWords = wordList.NumberOfWords;
                PercentExpired = wordListProgress.PercentExpired;
            }

            public string WordListId { get; private set; }

            public string OwnerId { get; private set; }

            public string Name { get; private set; }

            public int NumberOfWords { get; private set; }

            public int PercentDone { get; private set; }

            public int PercentExpired { get; private set; }
        }
    }
}