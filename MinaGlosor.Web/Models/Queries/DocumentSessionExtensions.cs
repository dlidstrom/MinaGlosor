using System.Collections.Generic;
using System.Linq;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries
{
    // TODO Try to replace code with this method.
    public static class DocumentSessionExtensions
    {
        public static TResult[] LoadAll<TResult, TIndex>(this DocumentSession session) where TIndex : AbstractIndexCreationTask, new()
        {
            var query = from word in session.Query<TResult, TIndex>()
                        select word;
            var words = new List<TResult>();
            var current = 0;
            while (true)
            {
                var subset = query.Skip(current).Take(128).ToArray();
                if (subset.Length == 0) break;
                words.AddRange(subset);
                current += subset.Length;
            }

            return words.ToArray();
        }
    }
}