using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;

namespace MinaGlosor.Web.Models.Domain.ProgressModel.Queries.Handlers
{
    public class GetProgressListByWordListIdQueryHandler : QueryHandlerBase<GetProgressListByWordListIdQuery, GetProgressListByWordListIdQuery.Result>
    {
        public override bool CanExecute(GetProgressListByWordListIdQuery query, User currentUser)
        {
            return true;
        }

        public override GetProgressListByWordListIdQuery.Result Handle(GetProgressListByWordListIdQuery query)
        {
            var linq = Session.Query<Progress, ProgressIndex>()
                              .Where(x => x.WordListId == query.WordListId)
                              .Select(x => x.Id);
            var ids = linq.ToArray();
            var result = new GetProgressListByWordListIdQuery.Result(ids);
            return result;
        }
    }
}