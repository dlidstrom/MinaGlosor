using System;
using System.Data.Entity;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetCreateAccountRequest : IQuery<GetCreateAccountRequest.Result>
    {
        private readonly string email;

        public GetCreateAccountRequest(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            this.email = email;
        }

        public async Task<Result> ExecuteAsync(IDbContext context)
        {
            var item = await context.CreateAccountRequests.SingleOrDefaultAsync(x => x.Email == email);
            return item == null ? null : new Result(item);
        }

        public class Result
        {
            public Result(CreateAccountRequest request)
            {
                ActivationCode = request.ActivationCode;
            }

            public Guid ActivationCode { get; private set; }
        }
    }
}