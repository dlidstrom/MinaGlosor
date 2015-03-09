using System;
using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Abstractions;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordListsQuery : IQuery<GetWordListsQuery.Result[]>
    {
        private readonly User user;

        public GetWordListsQuery(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            this.user = user;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public Result[] Execute(IDocumentSession session)
        {
            var wordLists = session.Query<WordListIndex.Result, WordListIndex>()
                                   .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                   .Where(x => x.OwnerId == user.Id)
                                   .ToArray();

            var results = new List<Result>();
            foreach (var wordList in wordLists)
            {
                var expiredCount = session.Query<WordScore, WordScoreIndex>()
                                          .Count(x => x.RepeatAfterDate < SystemTime.UtcNow && x.WordListId == wordList.Id);
                var result = new Result(wordList, expiredCount);
                results.Add(result);
            }

            var orderedResults = results.OrderByDescending(x => x.NumberOfWords > 0 ? 1 : 0)
                                        .ThenByDescending(x => x.PercentExpired)
                                        .ThenBy(x => x.PercentDone == 100 ? 1 : 0)
                                        .ThenBy(x => x.WordListId)
                                        .ToArray();
            return orderedResults;
        }

        public class Result
        {
            public Result(WordListIndex.Result wordList, int expiredCount)
            {
                if (wordList == null) throw new ArgumentNullException("wordList");

                WordListId = WordList.FromId(wordList.Id);
                OwnerId = User.FromId(wordList.OwnerId);
                Name = wordList.Name;
                NumberOfWords = wordList.NumberOfWords;
                PercentDone = (int)Math.Floor(100.0 * wordList.NumberOfWordScores / Math.Max(1, wordList.NumberOfWords));
                PercentExpired = (int)Math.Floor(100.0 * expiredCount / Math.Max(1, wordList.NumberOfWords));
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