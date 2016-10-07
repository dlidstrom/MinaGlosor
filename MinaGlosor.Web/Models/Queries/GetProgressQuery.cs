using System;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.ProgressModel;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetProgressQuery : IQuery<GetProgressQuery.Result>
    {
        public GetProgressQuery(string progressId)
        {
            if (progressId == null) throw new ArgumentNullException("progressId");
            ProgressId = progressId;
        }

        public string ProgressId { get; private set; }

        public class Result
        {
            public Result(Progress progress)
            {
            }
        }
    }
}