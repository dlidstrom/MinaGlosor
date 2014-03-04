using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetWordQuery : IQuery<GetWordQuery.Result>
    {
        private readonly string wordId;

        public GetWordQuery(string wordId)
        {
            this.wordId = wordId;
        }

        public Task<Result> ExecuteAsync(IDbContext session)
        {
            return null;
            //return new Result(session.Load<Word>(wordId));
        }

        public class Result
        {
            public Result(Word word)
            {
                WordId = word.Id;
                Text = word.Text;
                Definition = word.Definition;
            }

            public string Definition { get; set; }

            public string Text { get; set; }

            public int WordId { get; set; }
        }
    }
}