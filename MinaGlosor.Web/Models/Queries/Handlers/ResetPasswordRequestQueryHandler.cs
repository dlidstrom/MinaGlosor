using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class ResetPasswordRequestQueryHandler : IQueryHandler<GetResetPasswordRequestQuery, ResetPasswordRequest>
    {
        public IDocumentSession Session { get; set; }

        public bool CanExecute(GetResetPasswordRequestQuery query, User currentUser)
        {
            return true;
        }

        public ResetPasswordRequest Handle(GetResetPasswordRequestQuery query)
        {
            var linq = from x in Session.Query<ResetPasswordRequest, ResetPasswordRequestIndex>()
                       where x.ActivationCode == query.ActivationCode
                       select x;
            var result = linq.Single();
            return result;
        }
    }
}