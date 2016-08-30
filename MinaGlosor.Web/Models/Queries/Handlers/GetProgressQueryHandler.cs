using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetProgressQueryHandler : QueryHandlerBase<GetProgressQuery, GetProgressQuery.Result>
    {
        public override bool CanExecute(GetProgressQuery query, User currentUser)
        {
            return query.UserId == currentUser.Id;
        }

        public override GetProgressQuery.Result Handle(GetProgressQuery query)
        {
            var wordListProgresses = Session.Query<WordListProgress.Model, WordListProgressIndex>()
                                            .Where(x => x.OwnerId == query.UserId)
                                            .Skip((query.Page - 1) * query.ItemsPerPage)
                                            .Take(query.ItemsPerPage)
                                            .ToArray();
            var wordLists = Session.Load<WordList>(wordListProgresses.Select(x => x.WordListId))
                                   .ToDictionary(x => x.Id);
            var wordListProgressResults = wordListProgresses.Select(x => new GetProgressQuery.WordListProgressResult(x, wordLists[x.WordListId]))
                                                            .ToArray();
            var numberOfFavourites = Session.Query<WordFavourite, WordFavouriteIndex>()
                                            .Where(x => x.UserId == query.UserId)
                                            .Count(x => x.IsFavourite);
            var result = new GetProgressQuery.Result(wordListProgressResults, numberOfFavourites);
            return result;
        }
    }
}