using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetAdminUsersQueryHandler : QueryHandlerBase<GetAdminUsersQuery, GetAdminUsersQuery.Result>
    {
        public override bool CanExecute(GetAdminUsersQuery query, User currentUser)
        {
            return currentUser.IsAdmin;
        }

        public override GetAdminUsersQuery.Result Handle(GetAdminUsersQuery query)
        {
            var adminUsersQuery = from user in Session.Query<User, UserIndex>()
                                                      .Customize(x => x.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(30)))
                                  where user.Role == UserRole.Admin
                                  select user;
            var adminUsers = adminUsersQuery.ToArray();
            var ids = adminUsers.Select(x => x.Id).ToArray();
            var result = new GetAdminUsersQuery.Result(ids);
            return result;
        }
    }
}