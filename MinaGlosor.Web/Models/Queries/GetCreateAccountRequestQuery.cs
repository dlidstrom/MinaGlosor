using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetCreateAccountRequestQuery : IQuery<CreateAccountRequest>
    {
        private readonly Guid activationCode;

        public GetCreateAccountRequestQuery(Guid activationCode)
        {
            this.activationCode = activationCode;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public CreateAccountRequest Execute(IDocumentSession session)
        {
            var createAccountRequest = session.Query<CreateAccountRequest, CreateAccountRequestIndex>()
                                              .SingleOrDefault(x => x.ActivationCode == activationCode);
            return createAccountRequest;
        }
    }
}