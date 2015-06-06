using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var allResults = SearchTerm(session, word => word.Text);
            var resultsForDefinition = SearchTerm(session, word => word.Definition);
            foreach (var wordResult in resultsForDefinition)
            {
                allResults.Add(wordResult);
            }

            var result = new Result(allResults.ToArray());
            return result;
        }

        private SortedSet<WordResult> SearchTerm(IDocumentSession session, Expression<Func<Word, object>> expression)
        {
            var query = session.Query<Word, WordIndex>()
                               .Search(expression, q).ProjectFromIndexFieldsInto<WordResult>()
                               .Take(MaxResults);
            var resultSet = new SortedSet<WordResult>(query.ToArray(), WordResult.Comparer);
            if (resultSet.Count < MaxResults)
            {
                var suggestionQueryResults = query.Suggest();
                var results = session.Query<Word, WordIndex>()
                                     .Search(expression, string.Format("{0}*", q), escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
                                     .Search(expression, string.Join(" ", suggestionQueryResults.Suggestions))
                                     .ProjectFromIndexFieldsInto<WordResult>()
                                     .Take(MaxResults - resultSet.Count)
                                     .ToArray();
                foreach (var suggestedResult in results.Concat(results))
                {
                    resultSet.Add(suggestedResult);
                }
            }

            return resultSet;
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