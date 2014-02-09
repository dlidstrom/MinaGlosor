using System;
using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Infrastructure.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetWordListsQuery : IQuery<IEnumerable<GetWordListsQuery.Result>>
    {
        private readonly User currentUser;

        public GetWordListsQuery(User currentUser)
        {
            if (currentUser == null) throw new ArgumentNullException("currentUser");
            this.currentUser = currentUser;
        }

        public IEnumerable<Result> Execute(IDocumentSession session)
        {
            var items = session.Query<WordList, WordListIndex>()
                               .Where(x => x.OwnerId == currentUser.Id)
                               .OrderBy(x => x.Name)
                               .ToArray()
                               .Select(x => new Result(x));
            return items;
        }

        public class Result
        {
            public Result(WordList wordList)
            {
                if (wordList == null) throw new ArgumentNullException("wordList");
                Id = wordList.Id;
                Name = wordList.Name;
            }

            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}