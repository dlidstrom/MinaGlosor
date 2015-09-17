using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetAdminUsersQuery : IQuery<GetAdminUsersQuery.Result>
    {
        public class Result
        {
            public Result(string[] ids)
            {
                Ids = ids;
            }

            public string[] Ids { get; private set; }
        }
    }
}