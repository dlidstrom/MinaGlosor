using System.Data.Entity;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetWordQuery : IQuery<GetWordQuery.Result>
    {
        private readonly int id;

        public GetWordQuery(int id)
        {
            this.id = id;
        }

        public async Task<Result> ExecuteAsync(IDbContext context)
        {
            var word = await context.Words.SingleAsync(x => x.Id == id);
            return new Result(word);
        }

        public class Result
        {
            public Result(Word word)
            {
                Id = word.Id;
                WordListId = word.WordListId;
                Text = word.Text;
                Definition = word.Definition;
            }

            public int Id { get; private set; }

            public int WordListId { get; private set; }

            public string Text { get; private set; }

            public string Definition { get; private set; }
        }
    }
}