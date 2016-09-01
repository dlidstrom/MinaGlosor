using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetProgressQueryHandler : QueryHandlerBase<GetProgressQuery, GetProgressQuery.Result>
    {
        public override bool CanExecute(GetProgressQuery query, User currentUser)
        {
            return true;
        }

        public override GetProgressQuery.Result Handle(GetProgressQuery query)
        {
            var progress = Session.Load<Progress>(query.ProgressId);
            if (progress == null) return null;
            var result = new GetProgressQuery.Result(progress);
            return result;
        }
    }
}