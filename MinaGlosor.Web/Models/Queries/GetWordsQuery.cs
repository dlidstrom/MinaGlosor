using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordsQuery : IQuery<GetWordsResult>
    {
        public GetWordsQuery(string userId, string wordListId, int page, int itemsPerPage)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (wordListId == null) throw new ArgumentNullException("wordListId");

            // TODO Find better solution for this mess...
            WordListId = WordList.ToId(wordListId);
            UserId = userId;
            Page = page;
            ItemsPerPage = itemsPerPage;
        }

        public string UserId { get; private set; }

        public string WordListId { get; private set; }

        public int Page { get; private set; }

        public int ItemsPerPage { get; private set; }
    }
}