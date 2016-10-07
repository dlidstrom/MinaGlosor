using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Domain.ProgressModel.Queries
{
    public class GetProgressListByWordListIdQuery : IQuery<GetProgressListByWordListIdQuery.Result>
    {
        public GetProgressListByWordListIdQuery(string wordListId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            WordListId = wordListId;
        }

        public string WordListId { get; private set; }

        public class Result
        {
            public Result(string[] progressIds)
            {
                ProgressIds = progressIds;
            }

            public string[] ProgressIds { get; private set; }
        }
    }
}