using System.Globalization;
using System.Linq;
using MinaGlosor.Web.Infrastructure.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetWordListByIdQuery : IQuery<GetWordListByIdQuery.Result>
    {
        private readonly int id;

        public GetWordListByIdQuery(int id)
        {
            this.id = id;
        }

        public Result Execute(IDocumentSession session)
        {
            var stringifiedId = "WordLists-" + id;
            var wordList = session.Query<WordListIndex.Result, WordListIndex>()
                .SingleOrDefault(x => x.WordListId == stringifiedId);
            return wordList != null ? new Result(wordList) : null;
        }

        public class Result
        {
            public Result(WordListIndex.Result wordList)
            {
                Id = int.Parse(wordList.WordListId.Replace("WordLists-", string.Empty), CultureInfo.InvariantCulture);
                Name = wordList.WordListName;
                WordCount = wordList.WordCount;
            }

            public int Id { get; private set; }

            public string Name { get; private set; }

            public int WordCount { get; private set; }
        }
    }
}