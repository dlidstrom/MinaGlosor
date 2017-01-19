using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class SearchQueryHandler : IQueryHandler<SearchQuery, SearchQuery.Result>
    {
        private const int MaxResults = 30;

        public IDocumentSession Session { get; set; }

        public bool CanExecute(SearchQuery query, User currentUser)
        {
            return true;
        }

        public SearchQuery.Result Handle(SearchQuery query)
        {
            var index = 0;
            var textResults = SearchTerm(Session, word => word.Text, index, query.Q, query.UserId);
            index = textResults.Count;
            var definitionResults = SearchTerm(Session, word => word.Definition, index, query.Q, query.UserId);
            index += definitionResults.Count;
            var textSuggestions = new HashSet<SearchQuery.WordProjection>();
            var definitionSuggestions = new HashSet<SearchQuery.WordProjection>();

            if (textResults.Count < MaxResults)
            {
                textSuggestions = SearchSuggestions(word => word.Text, index, MaxResults - index, query.Q, query.UserId);
                index += textSuggestions.Count;
            }

            if (definitionResults.Count < MaxResults)
            {
                definitionSuggestions = SearchSuggestions(word => word.Definition, index, MaxResults - index, query.Q, query.UserId);
                index += definitionSuggestions.Count;
            }

            var combined = new HashSet<SearchQuery.WordProjection>(
                textResults.Concat(definitionResults).Concat(textSuggestions).Concat(definitionSuggestions),
                SearchQuery.WordProjection.EqualityComparer);
            var result = new SearchQuery.Result(combined.OrderBy(x => x.Index).ToArray());
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

        private HashSet<SearchQuery.WordProjection> SearchTerm(
            IDocumentSession session,
            Expression<Func<Word, object>> expression,
            int index,
            string q,
            string userId)
        {
            FieldHighlightings highlightings = null;
            var propertyName = GetPropertyInfo(expression).Name;
            var query = session.Query<Word, WordIndex>()
                               .Customize(x => x.Highlight(propertyName, 128, 1, out highlightings))
                               .Search(expression, q)
                               .Where(x => x.UserId == userId)
                               .ProjectFromIndexFieldsInto<SearchQuery.WordProjection>()
                               .Take(MaxResults);
            var results = query.ToArray();

            var resultSet = new HashSet<SearchQuery.WordProjection>(SearchQuery.WordProjection.EqualityComparer);
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

        private HashSet<SearchQuery.WordProjection> SearchSuggestions(
            Expression<Func<Word, object>> expression,
            int index,
            int maxCount,
            string q,
            string userId)
        {
            var suggestionResults = Session.Query<Word, WordIndex>()
                                           .Search(expression, q)
                                           .Take(maxCount)
                                           .Suggest();
            FieldHighlightings highlightings = null;
            var propertyName = GetPropertyInfo(expression).Name;
            var results = Session.Query<Word, WordIndex>()
                                 .Customize(x => x.Highlight(propertyName, 128, 1, out highlightings))
                                 .Search(
                                     expression,
                                     string.Format("{0}*", q),
                                     escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
                                 .Search(
                                     expression,
                                     string.Join(" ", suggestionResults.Suggestions))
                                 .Where(x => x.UserId == userId)
                                 .ProjectFromIndexFieldsInto<SearchQuery.WordProjection>()
                                 .Take(maxCount)
                                 .ToArray();

            var resultSet = new HashSet<SearchQuery.WordProjection>(SearchQuery.WordProjection.EqualityComparer);
            foreach (var result in results)
            {
                var fragments = highlightings.GetFragments(result.Id);
                var fragment = fragments.FirstOrDefault();
                if (fragment != null)
                {
                    var propertyInfo = result.GetType().GetProperty(propertyName);
                    propertyInfo.SetValue(result, fragment);
                }

                result.Index = index++;
                resultSet.Add(result);
            }

            return resultSet;
        }
    }
}