using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class UserQueryHandler :
        IQueryHandler<HasAdminUserQuery, bool>,
        IQueryHandler<GetUserByUsernameQuery, User>,
        IQueryHandler<GetUserByEmailQuery, User>,
        IQueryHandler<GetAdminUsersQuery, GetAdminUsersQuery.Result>
    {
        public IDocumentSession Session { get; set; }

        public bool CanExecute(HasAdminUserQuery query, User currentUser)
        {
            return true;
        }

        public bool Handle(HasAdminUserQuery query)
        {
            var anyAdminUser = Session.Query<User, UserIndex>().Any(x => x.Role == UserRole.Admin);
            return anyAdminUser;
        }

        public bool CanExecute(GetUserByUsernameQuery query, User currentUser)
        {
            return true;
        }

        public User Handle(GetUserByUsernameQuery query)
        {
            var existingUser = Session.Query<User, UserIndex>().SingleOrDefault(x => x.Username == query.Username);
            return existingUser;
        }

        public bool CanExecute(GetUserByEmailQuery query, User currentUser)
        {
            return true;
        }

        public User Handle(GetUserByEmailQuery query)
        {
            var user = Session.Query<User, UserIndex>().SingleOrDefault(x => x.Email == query.Email);
            return user;
        }

        public bool CanExecute(GetAdminUsersQuery query, User currentUser)
        {
            return currentUser.IsAdmin;
        }

        public GetAdminUsersQuery.Result Handle(GetAdminUsersQuery query)
        {
            var adminUsersQuery = from user in Session.Query<User, UserIndex>()
                                  where user.Role == UserRole.Admin
                                  select user;
            var adminUsers = adminUsersQuery.ToArray();
            var ids = adminUsers.Select(x => x.Id).ToArray();
            var result = new GetAdminUsersQuery.Result(ids);
            return result;
        }
    }
}