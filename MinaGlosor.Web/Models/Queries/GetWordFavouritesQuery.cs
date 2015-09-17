using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordFavouritesQuery : IQuery<GetWordsResult>
    {
        public GetWordFavouritesQuery(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            UserId = userId;
        }

        public string UserId { get; private set; }
    }
}