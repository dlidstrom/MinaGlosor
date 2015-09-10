using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetWordScoreIdsQueryHandler : QueryHandlerBase<GetWordScoreIdsQuery, GetWordScoreIdsQuery.Result>
    {
        public override bool CanExecute(User currentUser)
        {
            return true;
        }

        public override GetWordScoreIdsQuery.Result Handle(GetWordScoreIdsQuery query)
        {
            var linq = Session.Query<WordScore, WordScoreIndex>()
                              .Where(x => x.WordId == query.WordId)
                              .Select(x => x.Id);
            var ids = linq.ToArray();
            var result = new GetWordScoreIdsQuery.Result(ids);
            return result;
        }
    }
}