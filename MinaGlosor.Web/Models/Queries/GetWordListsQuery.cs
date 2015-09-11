using System;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordListsQuery : IQuery<GetWordListsQuery.Result>
    {
        public GetWordListsQuery(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            UserId = userId;
        }

        public string UserId { get; private set; }

        public class Result
        {
            public Result(WordListResult[] wordLists, int numberOfFavourites)
            {
                if (wordLists == null) throw new ArgumentNullException("wordLists");
                WordLists = wordLists;
                NumberOfFavourites = numberOfFavourites;
            }

            public WordListResult[] WordLists { get; private set; }

            public int NumberOfFavourites { get; private set; }
        }

        public class WordListResult
        {
            public WordListResult(WordListIndex.Result wordList, int expiredCount)
            {
                if (wordList == null) throw new ArgumentNullException("wordList");

                WordListId = WordList.FromId(wordList.Id);
                OwnerId = User.FromId(wordList.OwnerId);
                Name = wordList.Name;
                NumberOfWords = wordList.NumberOfWords;
                PercentDone = (int)Math.Floor(100.0 * wordList.NumberOfWordScores / Math.Max(1, wordList.NumberOfWords));
                PercentExpired = (int)Math.Floor(100.0 * expiredCount / Math.Max(1, wordList.NumberOfWords));
                Rank = int.Parse(WordListId);
            }

            public string WordListId { get; private set; }

            public string OwnerId { get; private set; }

            public string Name { get; private set; }

            public int NumberOfWords { get; private set; }

            public int PercentDone { get; private set; }

            public int PercentExpired { get; private set; }

            [JsonIgnore]
            public int Rank { get; private set; }
        }
    }
}