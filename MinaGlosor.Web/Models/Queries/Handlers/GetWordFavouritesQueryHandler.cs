using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetWordFavouritesQueryHandler : QueryHandlerBase<GetWordFavouritesQuery, GetWordFavouritesResult>
    {
        public override bool CanExecute(GetWordFavouritesQuery query, User currentUser)
        {
            return true;
        }

        public override GetWordFavouritesResult Handle(GetWordFavouritesQuery query)
        {
            RavenQueryStatistics stats;
            var linq = Session.Query<WordFavourite, WordFavouriteIndex>()
                              .Statistics(out stats)
                              .Skip((query.Page - 1) * query.ItemsPerPage)
                              .Take(query.ItemsPerPage)
                              .Where(x => x.IsFavourite && x.UserId == query.UserId);
            var favourites = linq.ToArray();
            var words = Session.Load<Word>(favourites.Select(x => x.WordId));
            var result = new GetWordFavouritesResult(
                words,
                stats.TotalResults,
                query.Page,
                query.ItemsPerPage);
            return result;
        }
    }
}