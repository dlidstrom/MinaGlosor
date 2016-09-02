using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordFavouritesQuery : IQuery<GetWordsResult>
    {
        public GetWordFavouritesQuery(string userId, int page, int itemsPerPage)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            UserId = userId;
            Page = page;
            ItemsPerPage = itemsPerPage;
        }

        public string UserId { get; private set; }

        public int Page { get; private set; }

        public int ItemsPerPage { get; private set; }
    }
}