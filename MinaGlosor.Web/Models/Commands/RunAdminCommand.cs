using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.AdminCommands;

namespace MinaGlosor.Web.Models.Commands
{
    public class RunAdminCommand : ICommand<object>
    {
        public RunAdminCommand(IAdminCommand adminCommand)
        {
            AdminCommand = adminCommand;
        }

        public IAdminCommand AdminCommand { get; private set; }
    }
}