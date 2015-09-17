using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetCreateAccountRequestQuery : IQuery<CreateAccountRequest>
    {
        public GetCreateAccountRequestQuery(Guid activationCode)
        {
            ActivationCode = activationCode;
        }

        public Guid ActivationCode { get; private set; }
    }
}