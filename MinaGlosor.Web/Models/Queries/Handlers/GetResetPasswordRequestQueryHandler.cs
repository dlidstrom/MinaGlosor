using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetResetPasswordRequestQueryHandler : QueryHandlerBase<GetResetPasswordRequestQuery, ResetPasswordRequest>
    {
        public override bool CanExecute(GetResetPasswordRequestQuery query, User currentUser)
        {
            return true;
        }

        public override ResetPasswordRequest Handle(GetResetPasswordRequestQuery query)
        {
            var linq = from x in Session.Query<ResetPasswordRequest, ResetPasswordRequestIndex>()
                       where x.ActivationCode == query.ActivationCode
                       select x;
            var result = linq.Single();
            return result;
        }
    }
}