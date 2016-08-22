using System.Linq;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class BrowseQuery : IQuery<BrowseQuery.Result>
    {
        public class Result
        {
            public Result(WordList[] wordLists)
            {
                WordLists = wordLists.Select(x => new WordListResult(x)).ToArray();
            }

            public WordListResult[] WordLists { get; set; }
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
    }
}