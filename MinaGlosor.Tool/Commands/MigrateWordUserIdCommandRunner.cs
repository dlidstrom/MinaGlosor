using MinaGlosor.Web.Models.AdminCommands;

namespace MinaGlosor.Tool.Commands
{
    public class MigrateWordUserIdCommandRunner : CommandRunner
    {
        protected override IAdminCommand CreateCommand(string username, string password, string[] args)
        {
            return new MigrateWordUserIdAdminCommand(username, password);
        }
    }
}