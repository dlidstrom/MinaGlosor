using System.ComponentModel.DataAnnotations;

namespace MinaGlosor.Web.Models.AdminCommands
{
    public abstract class AdminCommand : IAdminCommand
    {
        protected AdminCommand(string requestUsername, string requestPassword)
        {
            RequestUsername = requestUsername;
            RequestPassword = requestPassword;
        }

        [Required]
        public string RequestUsername { get; private set; }

        [Required]
        public string RequestPassword { get; private set; }
    }
}