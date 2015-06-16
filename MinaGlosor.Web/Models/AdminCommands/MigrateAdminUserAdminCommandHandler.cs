using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Models.AdminCommands
{
    public class MigrateAdminUserAdminCommandHandler : AbstractAdminCommandHandler<MigrateAdminUserAdminCommand>
    {
        public override object Run(MigrateAdminUserAdminCommand command)
        {
            var adminUsers = ExecuteQuery(new GetAdminUsersQuery());
            var result = ExecuteCommand(new RegisterAdminUsersCommand(adminUsers.Ids));
            return result;
        }
    }
}