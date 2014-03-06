using System.Data.Entity;
using System.Linq;
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

        public async Task<Result> ExecuteAsync(IDbContext context)
        {
            return await context.WordLists.Select(x => new Result
                {
                    Id = x.Id,
                    Name = x.Name,
                    WordCount = x.Words.Count()
                }).SingleOrDefaultAsync(x => x.Id == id);
        }

        public class Result
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public int WordCount { get; set; }
        }
    }
}