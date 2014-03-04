using System.Threading.Tasks;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetWordListByIdQuery : IQuery<GetWordListByIdQuery.Result>
    {
        private readonly int id;

        public GetWordListByIdQuery(int id)
        {
            this.id = id;
        }

        public Task<Result> ExecuteAsync(IDbContext session)
        {
            //var stringifiedId = "WordLists-" + id;
            //var wordList = session.Query<WordListIndex.Result, WordListIndex>()
            //    .SingleOrDefault(x => x.WordListId == stringifiedId);
            //return wordList != null ? new Result(wordList) : null;
            return null;
        }

        public class Result
        {
            public int Id { get; private set; }

            public string Name { get; private set; }

            public int WordCount { get; private set; }
        }
    }
}