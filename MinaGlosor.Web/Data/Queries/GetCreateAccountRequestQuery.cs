using System;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetCreateAccountRequestQuery : IQuery<CreateAccountRequest>
    {
        private readonly string activationCode;

        public GetCreateAccountRequestQuery(string activationCode)
        {
            if (activationCode == null) throw new ArgumentNullException("activationCode");
            this.activationCode = activationCode;
        }

        public Task<CreateAccountRequest> ExecuteAsync(IDbContext session)
        {
            //return session.Query<CreateAccountRequest, CreateAccountRequestIndex>()
            //              .FirstOrDefault(x => x.ActivationCode == activationCode);
            return null;
        }
    }
}