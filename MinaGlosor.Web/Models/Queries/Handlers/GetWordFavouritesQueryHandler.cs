using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetWordFavouritesQueryHandler : QueryHandlerBase<GetWordFavouritesQuery, GetWordsResult>
    {
        public override bool CanExecute(GetWordFavouritesQuery query, User currentUser)
        {
            return true;
        }

        public override GetWordsResult Handle(GetWordFavouritesQuery query)
        {
            var linq = from favourite in Session.Query<WordFavourite, WordFavouriteIndex>()
                       where favourite.IsFavourite && favourite.UserId == query.UserId
                       select favourite;
            var favourites = linq.ToArray();
            var words = Session.Load<Word>(favourites.Select(x => x.WordId));
            var result = new GetWordsResult(string.Empty, words);
            return result;
        }
    }
}