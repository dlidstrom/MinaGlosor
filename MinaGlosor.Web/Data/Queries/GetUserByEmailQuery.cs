using System;
using System.Linq;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetUserByEmailQuery : IQuery<GetUserByEmailQuery.Result>
    {
        private readonly string email;

        public GetUserByEmailQuery(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            this.email = email;
        }

        public Task<Result> ExecuteAsync(IDbContext context)
        {
            return Task.FromResult(new Result(context.Users.Single(x => x.Email == email)));
        }

        public class Result
        {
            private readonly User user;

            public Result(User user)
            {
                this.user = user;
            }

            public string Email
            {
                get { return user.Email; }
            }

            public bool ValidatePassword(string password)
            {
                return user.ValidatePassword(password);
            }
        }
    }
}