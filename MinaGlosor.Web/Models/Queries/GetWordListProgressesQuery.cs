using System;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordListProgressesQuery : IQuery<GetWordListProgressesQuery.Result>
    {
        public GetWordListProgressesQuery(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            UserId = userId;
        }

        public string UserId { get; private set; }

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