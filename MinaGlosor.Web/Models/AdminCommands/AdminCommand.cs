namespace MinaGlosor.Web.Models.AdminCommands
{
    public abstract class AdminCommand : IAdminCommand
    {
        protected AdminCommand(string requestUsername, string requestPassword)
        {
            RequestUsername = requestUsername;
            RequestPassword = requestPassword;
        }

        public string RequestUsername { get; private set; }

        public string RequestPassword { get; private set; }
    }
}