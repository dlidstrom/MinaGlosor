using System.Linq;
using MinaGlosor.Web.Data.Models;
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

        public Result[] Execute(IDocumentSession session)
        {
            return session.Query<Word, WordIndex>()
                          .Where(x => x.WordListId == wordListId)
                          .AsProjection<Result>()
                          .ToArray();
        }

        public class Result
        {
            public int WordListId { get; set; }

            public string Text { get; set; }
        }
    }
}