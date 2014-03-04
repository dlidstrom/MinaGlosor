using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetWordListsQuery :
        QueryForEtagBase<GetWordListsQuery.Result>,
        IQuery<IEnumerable<GetWordListsQuery.Result>>
    {
        private readonly User currentUser;

        public GetWordListsQuery(User currentUser)
        {
            if (currentUser == null) throw new ArgumentNullException("currentUser");
            this.currentUser = currentUser;
        }

        public async Task<IEnumerable<Result>> ExecuteAsync(IDbContext context)
        {
            var wordLists = await context.WordLists
                                         .Where(x => x.OwnerId == currentUser.Id)
                                         .Select(x => new Result
                                             {
                                                 Id = x.Id,
                                                 Name = x.Name,
                                                 WordCount = x.Words.Count()
                                             })
                                         .ToArrayAsync();
            return wordLists;
            //var query = GetQuery(session);
            //var items = query.Where(x => x.OwnerId == currentUser.Id)
            //                 .OrderBy(x => x.WordListName)
            //                 .ToArray()
            //                 .Select(x => new Result(x));
            //return items;
        }

        protected override IRavenQueryable<Result> GetQuery(IDocumentSession session)
        {
            return null;
        }

        public class Result
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public int WordCount { get; set; }
        }
    }
}