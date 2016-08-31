using System.Collections.Generic;
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
            public Result(
                WordList[] wordLists,
                Dictionary<string, User> ownersDict,
                int totalItems,
                int currentPage,
                int itemsPerPage)
            {
                WordLists = wordLists.Select(x => new WordListResult(x, ownersDict[x.OwnerId])).ToArray();
                Paging = new Paging(totalItems, currentPage, itemsPerPage);
            }

            public WordListResult[] WordLists { get; private set; }

            public Paging Paging { get; private set; }
        }

        public class WordListResult
        {
            public WordListResult(WordList wordList, User user)
            {
                WordListId = WordList.FromId(wordList.Id);
                Name = wordList.Name;
                NumberOfWords = wordList.NumberOfWords;
                Username = user.Username;
                GravatarHash = user.GetGravatarHash();
            }

            public string WordListId { get; private set; }

            public string Name { get; private set; }

            public int NumberOfWords { get; private set; }

            public string Username { get; private set; }

            public string GravatarHash { get; private set; }
        }
    }
}