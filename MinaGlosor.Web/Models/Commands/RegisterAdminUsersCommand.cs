using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class RegisterAdminUsersCommand : ICommand<RegisterAdminUsersCommand.Result>
    {
        public RegisterAdminUsersCommand(string[] adminUserIds)
        {
            AdminUserIds = adminUserIds;
        }

        public string[] AdminUserIds { get; private set; }

        public class Result
        {
            public Result(string[] migratedUsers)
            {
                MigratedUsers = migratedUsers;
            }

            public string[] MigratedUsers { get; private set; }
        }
    }
}