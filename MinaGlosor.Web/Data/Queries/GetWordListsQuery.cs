using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Infrastructure.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetWordListsQuery :
        QueryForEtagBase<WordListIndex.Result>,
        IQuery<IEnumerable<GetWordListsQuery.Result>>
    {
        private readonly User currentUser;

        public GetWordListsQuery(User currentUser)
        {
            if (currentUser == null) throw new ArgumentNullException("currentUser");
            this.currentUser = currentUser;
        }

        public IEnumerable<Result> Execute(IDocumentSession session)
        {
            var query = GetQuery(session);
            var items = query.Where(x => x.OwnerId == currentUser.Id)
                             .OrderBy(x => x.WordListName)
                             .ToArray()
                             .Select(x => new Result(x));
            return items;
        }

        protected override IRavenQueryable<WordListIndex.Result> GetQuery(IDocumentSession session)
        {
            return session.Query<WordListIndex.Result, WordListIndex>();
        }

        public class Result
        {
            public Result(WordListIndex.Result result)
            {
                Id = int.Parse(result.WordListId.Replace("WordLists-", string.Empty), CultureInfo.InvariantCulture);
                Name = result.WordListName;
                WordCount = result.WordCount;
            }

            public int Id { get; private set; }

            public string Name { get; private set; }

            public int WordCount { get; private set; }
        }
    }
}