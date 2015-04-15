using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetResetPasswordRequestQuery : IQuery<ResetPasswordRequest>
    {
        private readonly string activationCode;

        public GetResetPasswordRequestQuery(string activationCode)
        {
            if (activationCode == null) throw new ArgumentNullException("activationCode");
            this.activationCode = activationCode;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public ResetPasswordRequest Execute(IDocumentSession session)
        {
            var query = from x in session.Query<ResetPasswordRequest, ResetPasswordRequestIndex>()
                        where x.ActivationCode == activationCode
                        select x;
            var result = query.Single();
            return result;
        }
    }
}