using System.Linq;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Infrastructure.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetCreateAccountRequestQuery : IQuery<CreateAccountRequest>
    {
        private readonly string activationCode;

        public GetCreateAccountRequestQuery(string activationCode)
        {
            this.activationCode = activationCode;
        }

        public CreateAccountRequest Execute(IDocumentSession session)
        {
            return session.Query<CreateAccountRequest, CreateAccountRequestIndex>()
                          .FirstOrDefault(x => x.ActivationCode == activationCode);
        }
    }
}