using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Abstractions;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetWordListsQueryHandler : QueryHandlerBase<GetWordListsQuery, GetWordListsQuery.Result>
    {
        public override bool CanExecute(GetWordListsQuery query, User currentUser)
        {
            return true;
        }

        public override GetWordListsQuery.Result Handle(GetWordListsQuery query)
        {
            var wordLists = Session.Query<WordListIndex.Result, WordListIndex>()
                                   .Where(x => x.OwnerId == query.UserId)
                                   .ToArray();

            var wordListResults = new List<GetWordListsQuery.WordListResult>();
            foreach (var wordList in wordLists)
            {
                var expiredCount = Session.Query<WordScore, WordScoreIndex>()
                                          .Count(x => x.RepeatAfterDate < SystemTime.UtcNow && x.WordListId == wordList.Id);
                var wordListResult = new GetWordListsQuery.WordListResult(wordList, expiredCount);
                wordListResults.Add(wordListResult);
            }

            var orderedResults = wordListResults.OrderBy(x => x.GetValuesForComparison()).ToArray();

            // favourites
            var numberOfFavourites = Session.Query<WordFavourite, WordFavouriteIndex>()
                                            .Where(x => x.UserId == query.UserId)
                                            .Count(x => x.IsFavourite);
            var result = new GetWordListsQuery.Result(orderedResults, numberOfFavourites);

            return result;
        }
    }
}