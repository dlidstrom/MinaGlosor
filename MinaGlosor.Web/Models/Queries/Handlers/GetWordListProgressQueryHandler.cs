using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetWordListProgressQueryHandler : QueryHandlerBase<GetWordListProgressQuery, GetWordListProgressQuery.Result>
    {
        public override bool CanExecute(GetWordListProgressQuery query, User currentUser)
        {
            return true;
        }

        public override GetWordListProgressQuery.Result Handle(GetWordListProgressQuery query)
        {
            var progress = Session.Load<Progress>(query.ProgressId);
            var result = new GetWordListProgressQuery.Result(progress);
            return result;
        }
    }
}