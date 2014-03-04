using System;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetUserQuery : IQuery<GetUserQuery.Result>
    {
        private readonly string id;

        public GetUserQuery(string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            this.id = id;
        }

        public Task<Result> ExecuteAsync(IDbContext context)
        {
            return null;
            //var user = session.Load<User>(id);
            //return user != null ? new Result(user) : null;
        }

        public class Result
        {
            public Result(User user)
            {
                if (user == null) throw new ArgumentNullException("user");
            }
        }
    }
}