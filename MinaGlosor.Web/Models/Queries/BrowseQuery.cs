using System.Linq;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class BrowseQuery : IQuery<BrowseQuery.Result>
    {
        public BrowseQuery(int page, int itemsPerPage)
        {
            Page = page;
            ItemsPerPage = itemsPerPage;
        }

        public int Page { get; private set; }

        public int ItemsPerPage { get; private set; }

        public class Result
        {
            public Result(WordList[] wordLists, int totalItems, int currentPage, int itemsPerPage)
            {
                WordLists = wordLists.Select(x => new WordListResult(x)).ToArray();
                Paging = new Paging(totalItems, currentPage, itemsPerPage);
            }

            public WordListResult[] WordLists { get; private set; }

            public Paging Paging { get; private set; }
        }

        public class WordListResult
        {
            public WordListResult(WordList wordList)
            {
                WordListId = WordList.FromId(wordList.Id);
                Name = wordList.Name;
            }

            public string WordListId { get; private set; }

            public string Name { get; private set; }
        }

        public class Paging
        {
            public Paging(int totalItems, int currentPage, int itemsPerPage)
            {
                TotalItems = totalItems;
                CurrentPage = currentPage;
                ItemsPerPage = itemsPerPage;
            }

            public int TotalItems { get; private set; }

            public int CurrentPage { get; private set; }

            public int ItemsPerPage { get; private set; }
        }
    }
}