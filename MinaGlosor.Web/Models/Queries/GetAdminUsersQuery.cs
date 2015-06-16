using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetAdminUsersQuery : IQuery<GetAdminUsersQuery.Result>
    {
        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return currentUser.IsAdmin;
        }

        public Result Execute(IDocumentSession session)
        {
            var adminUsersQuery = from user in session.Query<User, UserIndex>()
                                                      .Customize(x => x.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(30)))
                                  where user.Role == UserRole.Admin
                                  select user;
            var adminUsers = adminUsersQuery.ToArray();
            var ids = adminUsers.Select(x => x.Id).ToArray();
            var result = new Result(ids);
            return result;
        }

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