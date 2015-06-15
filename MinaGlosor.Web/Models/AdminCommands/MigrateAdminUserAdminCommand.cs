namespace MinaGlosor.Web.Models.AdminCommands
{
    public class MigrateAdminUserAdminCommand : AdminCommand
    {
        public MigrateAdminUserAdminCommand(string requestUsername, string requestPassword)
            : base(requestUsername, requestPassword)
        {
        }
    }
}