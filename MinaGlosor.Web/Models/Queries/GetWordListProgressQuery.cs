using System;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordListProgressQuery : IQuery<GetWordListProgressQuery.Result>
    {
        public GetWordListProgressQuery(string progressId)
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