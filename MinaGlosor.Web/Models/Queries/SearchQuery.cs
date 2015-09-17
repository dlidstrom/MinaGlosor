using System;
using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class SearchQuery : IQuery<SearchQuery.Result>
    {
        public SearchQuery(string q, string userId)
        {
            if (q == null) throw new ArgumentNullException("q");
            if (userId == null) throw new ArgumentNullException("userId");
            Q = q;
            UserId = userId;
        }

        public string Q { get; private set; }

        public string UserId { get; private set; }

        public class WordProjection
        {
            private static readonly IEqualityComparer<WordProjection> IdEqualityComparerInstance
                = new IdEqualityComparer();

            public static IEqualityComparer<WordProjection> EqualityComparer
            {
                get { return IdEqualityComparerInstance; }
            }

            public string Id { get; set; }

            public string Text { get; set; }

            public string Definition { get; set; }

            // for sorting purposes
            public int Index { get; set; }

            private sealed class IdEqualityComparer : IEqualityComparer<WordProjection>
            {
                public bool Equals(WordProjection x, WordProjection y)
                {
                    return x.Id.Equals(y.Id);
                }

                public int GetHashCode(WordProjection obj)
                {
                    return obj.Id.GetHashCode();
                }
            }
        }

        public class Result
        {
            public Result(WordProjection[] words)
            {
                if (words == null) throw new ArgumentNullException("words");
                Words = words.Select(x => new WordResult(x)).ToArray();
            }

            public WordResult[] Words { get; private set; }

            public class WordResult
            {
                public WordResult(WordProjection wordProjection)
                {
                    if (wordProjection == null) throw new ArgumentNullException("wordProjection");
                    WordId = Word.FromId(wordProjection.Id);
                    Text = wordProjection.Text;
                    Definition = wordProjection.Definition;
                }

                public string WordId { get; private set; }

                public string Text { get; private set; }

                public string Definition { get; private set; }
            }
        }
    }
}