using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordsQuery : IQuery<GetWordsResult>
    {
        public GetWordsQuery(string wordListId, int page, int itemsPerPage)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");

            // TODO Find better solution for this mess...
            WordListId = WordList.ToId(wordListId);
            Page = page;
            ItemsPerPage = itemsPerPage;
        }

        public string WordListId { get; private set; }

        public int Page { get; private set; }

        public int ItemsPerPage { get; private set; }
    }
}