using MinaGlosor.Web.Models.AdminCommands;

namespace MinaGlosor.Tool.Commands
{
    public class MigrateAdminUserCommandRunner : CommandRunner
    {
        protected override IAdminCommand CreateCommand(string username, string password, string[] args)
        {
            return new MigrateAdminUserAdminCommand(username, password);
        }
    }
}