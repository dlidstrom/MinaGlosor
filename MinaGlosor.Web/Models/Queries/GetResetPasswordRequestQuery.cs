using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetResetPasswordRequestQuery : IQuery<ResetPasswordRequest>
    {
        public GetResetPasswordRequestQuery(string activationCode)
        {
            if (activationCode == null) throw new ArgumentNullException("activationCode");
            ActivationCode = activationCode;
        }

        public string ActivationCode { get; private set; }
    }
}