using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class CreateAccountRequestQueryHandler : IQueryHandler<GetCreateAccountRequestQuery, CreateAccountRequest>
    {
        public IDocumentSession Session { get; set; }

        public bool CanExecute(GetCreateAccountRequestQuery query, User currentUser)
        {
            return true;
        }

        public CreateAccountRequest Handle(GetCreateAccountRequestQuery query)
        {
            var createAccountRequest = Session.Query<CreateAccountRequest, CreateAccountRequestIndex>()
                                              .SingleOrDefault(x => x.ActivationCode == query.ActivationCode);
            return createAccountRequest;
        }
    }
}