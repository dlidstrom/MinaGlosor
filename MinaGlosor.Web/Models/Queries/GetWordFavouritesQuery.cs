using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordFavouritesQuery : IQuery<GetWordsResult>
    {
        public GetWordFavouritesQuery(string currentUserId, string userId, int page, int itemsPerPage)
        {
            if (currentUserId == null) throw new ArgumentNullException("currentUserId");
            if (userId == null) throw new ArgumentNullException("userId");
            CurrentUserId = currentUserId;
            UserId = userId;
            Page = page;
            ItemsPerPage = itemsPerPage;
        }

        public string CurrentUserId { get; private set; }

        public string UserId { get; private set; }

        public int Page { get; private set; }

        public int ItemsPerPage { get; private set; }
    }
}