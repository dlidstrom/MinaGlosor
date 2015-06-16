namespace MinaGlosor.Web.Models.AdminCommands
{
    public class MigrateWordUserIdAdminCommand : AdminCommand
    {
        public MigrateWordUserIdAdminCommand(string requestUsername, string requestPassword)
            : base(requestUsername, requestPassword)
        {
        }
    }
}