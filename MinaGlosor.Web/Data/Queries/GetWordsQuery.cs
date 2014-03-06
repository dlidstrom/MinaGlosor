using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetWordsQuery : IQuery<IEnumerable<GetWordsQuery.Result>>
    {
        private readonly int wordListId;

        public GetWordsQuery(int wordListId)
        {
            this.wordListId = wordListId;
        }

        public async Task<IEnumerable<Result>> ExecuteAsync(IDbContext context)
        {
            var words = await context.Words.Where(x => x.WordListId == wordListId).ToArrayAsync();
            return words.Select(x => new Result(x));
        }

        public class Result
        {
            public Result(Word word)
            {
                Text = word.Text;
                Definition = word.Definition;
                EasynessFactor = EasynessFactor;
            }

            public string Text { get; private set; }

            public string Definition { get; private set; }

            public double EasynessFactor { get; private set; }
        }
    }
}