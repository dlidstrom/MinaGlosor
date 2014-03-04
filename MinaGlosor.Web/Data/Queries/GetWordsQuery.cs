using System.Linq;
using MinaGlosor.Web.Infrastructure.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetWordsQuery : IQuery<GetWordsQuery.Result[]>
    {
        private readonly string wordListId;

        public GetWordsQuery(string wordListId)
        {
            this.wordListId = wordListId;
        }

        public GetWordsQuery(int wordListId)
            : this("WordLists-" + wordListId)
        {
        }

        public Result[] Execute(IDocumentSession session)
        {
            return session.Query<WordsIndex.Result, WordsIndex>()
                          .Where(x => x.WordListId == wordListId)
                          .OrderBy(x => x.EasynessFactor)
                          .ThenBy(x => x.Text)
                          .AsProjection<Result>()
                          .ToArray();
        }

        public class Result
        {
            public string Text { get; set; }

            public string Definition { get; set; }

            public double EasynessFactor { get; set; }
        }
    }
}