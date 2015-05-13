using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordFavouritesQuery : IQuery<GetWordsResult>
    {
        private readonly string userId;

        public GetWordFavouritesQuery(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            this.userId = userId;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public GetWordsResult Execute(IDocumentSession session)
        {
            var query = from favourite in session.Query<WordFavourite, WordFavouriteIndex>()
                        where favourite.IsFavourite && favourite.UserId == userId
                        select favourite;
            var favourites = query.ToArray();
            var words = session.Load<Word>(favourites.Select(x => x.WordId));
            var result = new GetWordsResult(string.Empty, words);
            return result;
        }
    }
}