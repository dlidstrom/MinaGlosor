using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;
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
            RavenQueryStatistics stats;
            var progresses = Session.Query<Progress, WordListProgressIndex>()
                                    .Statistics(out stats)
                                    .Where(x => x.OwnerId == query.UserId)
                                    .Skip((query.Page - 1) * query.ItemsPerPage)
                                    .Take(query.ItemsPerPage)
                                    .ToArray();
            var wordLists = Session.Load<WordList>(progresses.Select(x => x.WordListId))
                                   .ToDictionary(x => x.Id);
            var progressResults = progresses.Select(x => new GetProgressQuery.ProgressResult(x, wordLists[x.WordListId]))
                                            .ToArray();
            var numberOfFavourites = Session.Query<WordFavourite, WordFavouriteIndex>()
                                            .Where(x => x.UserId == query.UserId)
                                            .Count(x => x.IsFavourite);
            var result = new GetProgressQuery.Result(
                progressResults,
                numberOfFavourites,
                stats.TotalResults,
                query.Page,
                query.ItemsPerPage);
            return result;
        }
    }
}