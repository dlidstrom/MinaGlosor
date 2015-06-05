using System;
using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class SearchQuery : IQuery<SearchQuery.Result>
    {
        private const int MaxResults = 30;
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
            var query = session.Query<Word, WordIndex>()
                               .Search(x => x.Text, q)
                               .Search(x => x.Definition, q)
                               .ProjectFromIndexFieldsInto<WordResult>()
                               .Take(MaxResults);
            var results = new SortedSet<WordResult>(query.ToArray(), WordResult.Comparer);
            if (results.Count < MaxResults)
            {
                var suggestionQueryResults = query.Suggest();
                var suggestedResults = session.Query<Word, WordIndex>()
                                              .Search(
                                                  x => x.Text,
                                                  string.Format("{0}*", q),
                                                  escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
                                              .Search(
                                                  x => x.Definition,
                                                  string.Format("{0}*", q),
                                                  escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
                                              .Search(x => x.Text, string.Join(" ", suggestionQueryResults.Suggestions))
                                              .Search(x => x.Definition, string.Join(" ", suggestionQueryResults.Suggestions))
                                              .ProjectFromIndexFieldsInto<WordResult>()
                                              .Take(MaxResults - results.Count)
                                              .ToArray();
                foreach (var suggestedResult in suggestedResults)
                {
                    results.Add(suggestedResult);
                }
            }

            var result = new Result(results.ToArray());
            return result;
        }

        public class WordResult
        {
            private static readonly IComparer<WordResult> IdComparerInstance = new IdEqualityComparer();

            public static IComparer<WordResult> Comparer
            {
                get { return IdComparerInstance; }
            }

            public string Id { get; set; }

            public string Text { get; set; }

            public string Definition { get; set; }

            private sealed class IdEqualityComparer : IComparer<WordResult>
            {
                public int Compare(WordResult x, WordResult y)
                {
                    return string.Compare(x.Id, y.Id, StringComparison.Ordinal);
                }
            }
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