using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
            var index = 0;
            var textResults = SearchTerm(session, word => word.Text, index);
            index = textResults.Count;
            var definitionResults = SearchTerm(session, word => word.Definition, index);
            index += definitionResults.Count;
            var textSuggestions = new HashSet<WordResult>();
            var definitionSuggestions = new HashSet<WordResult>();

            if (textResults.Count < MaxResults)
            {
                textSuggestions = SearchSuggestions(session, word => word.Text, index, MaxResults - index);
                index += textSuggestions.Count;
            }

            if (definitionResults.Count < MaxResults)
            {
                definitionSuggestions = SearchSuggestions(session, word => word.Definition, index, MaxResults - index);
                index += definitionSuggestions.Count;
            }

            var combined = new HashSet<WordResult>(
                textResults.Concat(definitionResults).Concat(textSuggestions).Concat(definitionSuggestions),
                WordResult.EqualityComparer);
            var result = new Result(combined.OrderBy(x => x.Index).ToArray());
            return result;
        }

        private static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            var type = typeof(TSource);

            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
            {
                var message = string.Format("Expression '{0}' refers to a method, not a property.", propertyLambda);
                throw new ArgumentException(message);
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                var message = string.Format("Expression '{0}' refers to a field, not a property.", propertyLambda);
                throw new ArgumentException(message);
            }

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
            {
                var message = string.Format("Expresion '{0}' refers to a property that is not from type {1}.", propertyLambda, type);
                throw new ArgumentException(message);
            }

            return propInfo;
        }

        private HashSet<WordResult> SearchTerm(
            IDocumentSession session,
            Expression<Func<Word, object>> expression,
            int index)
        {
            FieldHighlightings highlightings = null;
            var propertyName = GetPropertyInfo(expression).Name;
            var query = session.Query<Word, WordIndex>()
                               .Customize(x => x.Highlight(propertyName, 128, 1, out highlightings))
                               .Search(expression, q)
                               .ProjectFromIndexFieldsInto<WordResult>()
                               .Take(MaxResults);
            var results = query.ToArray();

            var resultSet = new HashSet<WordResult>(WordResult.EqualityComparer);
            foreach (var result in results)
            {
                var fragments = highlightings.GetFragments(result.Id);
                var fragment = fragments.FirstOrDefault();
                var propertyInfo = result.GetType().GetProperty(propertyName);
                if (fragment != null)
                {
                    propertyInfo.SetValue(result, fragment);
                }

                result.Index = index++;
                resultSet.Add(result);
            }

            return resultSet;
        }

        private HashSet<WordResult> SearchSuggestions(IDocumentSession session, Expression<Func<Word, object>> expression, int index, int maxCount)
        {
            var suggestionResults = session.Query<Word, WordIndex>()
                                           .Search(expression, q)
                                           .Take(maxCount)
                                           .Suggest();
            FieldHighlightings highlightings = null;
            var propertyName = GetPropertyInfo(expression).Name;
            var results = session.Query<Word, WordIndex>()
                                 .Customize(x => x.Highlight(propertyName, 128, 1, out highlightings))
                                 .Search(
                                    expression,
                                    string.Format("{0}*", q),
                                    escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
                                 .Search(
                                    expression,
                                    string.Join(" ", suggestionResults.Suggestions))
                                 .ProjectFromIndexFieldsInto<WordResult>()
                                 .Take(maxCount)
                                 .ToArray();

            var resultSet = new HashSet<WordResult>(WordResult.EqualityComparer);
            foreach (var result in results)
            {
                var fragments = highlightings.GetFragments(result.Id);
                var fragment = fragments.FirstOrDefault();
                var propertyInfo = result.GetType().GetProperty(propertyName);
                if (fragment != null)
                {
                    propertyInfo.SetValue(result, fragment);
                }

                result.Index = index++;
                resultSet.Add(result);
            }

            return resultSet;
        }

        public class WordResult
        {
            private static readonly IEqualityComparer<WordResult> IdEqualityComparerInstance = new IdEqualityComparer();

            public static IEqualityComparer<WordResult> EqualityComparer
            {
                get { return IdEqualityComparerInstance; }
            }

            public string Id { get; set; }

            public string Text { get; set; }

            public string Definition { get; set; }

            // for sorting purposes
            public int Index { get; set; }

            private sealed class IdEqualityComparer : IEqualityComparer<WordResult>
            {
                public bool Equals(WordResult x, WordResult y)
                {
                    return x.Id.Equals(y.Id);
                }

                public int GetHashCode(WordResult obj)
                {
                    return obj.Id.GetHashCode();
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