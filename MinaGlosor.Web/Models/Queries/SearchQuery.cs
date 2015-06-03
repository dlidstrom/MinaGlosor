using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class SearchQuery : IQuery<SearchQuery.Result>
    {
        private readonly string q;

        public SearchQuery(string q, string userId)
        {
            this.q = q;
            //this.userId = userId;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public Result Execute(IDocumentSession session)
        {
            var results = session.Query<Word, WordIndex>()
                                 .Search(x => x.Text, q)
                                 .ProjectFromIndexFieldsInto<WordResult>()
                                 .Take(10)
                                 .ToArray();
            var result = new Result(results);
            return result;
        }

        public class WordResult
        {
            public string Id { get; set; }

            public string Text { get; set; }

            public string Definition { get; set; }
        }

        public class Result
        {
            public Result(WordResult[] words)
            {
                if (words == null) throw new ArgumentNullException("words");
                Words = words;
            }

            public WordResult[] Words { get; private set; }
        }
    }
}