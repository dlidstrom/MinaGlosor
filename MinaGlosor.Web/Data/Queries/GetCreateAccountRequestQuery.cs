using System;
using System.Data.Entity;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetCreateAccountRequestQuery : IQuery<GetCreateAccountRequestQuery.Result>
    {
        private readonly Guid activationCode;

        public GetCreateAccountRequestQuery(Guid activationCode)
        {
            if (activationCode == null) throw new ArgumentNullException("activationCode");
            this.activationCode = activationCode;
        }

        public async Task<Result> ExecuteAsync(IDbContext context)
        {
            var item = await context.CreateAccountRequests.SingleOrDefaultAsync(x => x.ActivationCode == activationCode);
            return item != null ? new Result(item) : null;
        }

        public class Result
        {
            private readonly CreateAccountRequest createAccountRequest;

            public Result(CreateAccountRequest createAccountRequest)
            {
                if (createAccountRequest == null) throw new ArgumentNullException("createAccountRequest");
                this.createAccountRequest = createAccountRequest;
            }

            public string Email
            {
                get { return createAccountRequest.Email; }
            }

            public bool HasBeenUsed()
            {
                return createAccountRequest.HasBeenUsed();
            }

            public void MarkAsUsed()
            {
                createAccountRequest.MarkAsUsed();
            }
        }
    }
}