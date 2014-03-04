using System.Threading.Tasks;

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

        public Task<Result[]> ExecuteAsync(IDbContext context)
        {
            //return session.Query<WordsIndex.Result, WordsIndex>()
            //              .Where(x => x.WordListId == wordListId)
            //              .OrderBy(x => x.EasynessFactor)
            //              .ThenBy(x => x.Text)
            //              .AsProjection<Result>()
            //              .ToArray();
            return null;
        }

        public class Result
        {
            public string Text { get; set; }

            public string Definition { get; set; }

            public double EasynessFactor { get; set; }
        }
    }
}