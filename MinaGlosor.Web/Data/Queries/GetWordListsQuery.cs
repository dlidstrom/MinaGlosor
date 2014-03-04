using System;
using System.Collections.Generic;
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

        public IEnumerable<Result> Execute(IDbContext session)
        {
            //var query = GetQuery(session);
            //var items = query.Where(x => x.OwnerId == currentUser.Id)
            //                 .OrderBy(x => x.WordListName)
            //                 .ToArray()
            //                 .Select(x => new Result(x));
            //return items;
            return null;
        }

        protected override IRavenQueryable<Result> GetQuery(IDocumentSession session)
        {
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