using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetCreateAccountRequestQueryHandler : QueryHandlerBase<GetCreateAccountRequestQuery, CreateAccountRequest>
    {
        public override bool CanExecute(GetCreateAccountRequestQuery query, User currentUser)
        {
            return true;
        }

        public override CreateAccountRequest Handle(GetCreateAccountRequestQuery query)
        {
            var createAccountRequest = Session.Query<CreateAccountRequest, CreateAccountRequestIndex>()
                                              .SingleOrDefault(x => x.ActivationCode == query.ActivationCode);
            return createAccountRequest;
        }
    }
}