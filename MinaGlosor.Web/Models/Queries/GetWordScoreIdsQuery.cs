using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordScoreIdsQuery : IQuery<GetWordScoreIdsQuery.Result>
    {
        public GetWordScoreIdsQuery(string wordId)
        {
            WordId = wordId;
        }

        public string WordId { get; private set; }

        public class Result
        {
            public Result(string[] ids)
            {
                if (ids == null) throw new ArgumentNullException("ids");
                Ids = ids;
            }

            public string[] Ids { get; private set; }
        }
    }
}