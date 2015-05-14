using System;
using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Newtonsoft.Json;
using Raven.Abstractions;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordListsQuery : IQuery<GetWordListsQuery.Result>
    {
        private readonly string userId;

        public GetWordListsQuery(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            this.userId = userId;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public Result Execute(IDocumentSession session)
        {
            var wordLists = session.Query<WordListIndex.Result, WordListIndex>()
                                   .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                   .Where(x => x.OwnerId == userId)
                                   .ToArray();

            var wordListResults = new List<WordListResult>();
            foreach (var wordList in wordLists)
            {
                var expiredCount = session.Query<WordScore, WordScoreIndex>()
                                          .Count(x => x.RepeatAfterDate < SystemTime.UtcNow && x.WordListId == wordList.Id);
                var wordListResult = new WordListResult(wordList, expiredCount);
                wordListResults.Add(wordListResult);
            }

            var orderedResults = wordListResults.OrderByDescending(x => x.NumberOfWords > 0 ? 1 : 0)
                                                .ThenBy(x => x.PercentDone == 100 ? 1 : 0)
                                                .ThenByDescending(x => x.PercentExpired)
                                                .ThenBy(x => x.Rank)
                                                .ToArray();

            // favourites
            var numberOfFavourites = session.Query<WordFavourite, WordFavouriteIndex>()
                .Where(x => x.UserId == userId)
                .Count(x => x.IsFavourite);
            var result = new Result(orderedResults, numberOfFavourites);

            return result;
        }

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