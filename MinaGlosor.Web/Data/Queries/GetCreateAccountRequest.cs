using System;
using System.Linq;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Infrastructure.Indexes;
using Raven.Client;

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

        public Result Execute(IDbContext session)
        {
            var request = session.Query<CreateAccountRequest, CreateAccountRequestIndex>()
                                 .FirstOrDefault(x => x.Email == email);
            return request != null ? new Result(request) : null;
        }

        public class Result
        {
            public Result(CreateAccountRequest request)
            {
                ActivationCode = request.ActivationCode;
            }

            public string ActivationCode { get; set; }
        }
    }
}