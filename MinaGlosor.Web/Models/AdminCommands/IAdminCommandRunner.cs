namespace MinaGlosor.Web.Models.AdminCommands
{
    public interface IAdminCommandRunner
    {
        void Run(IAdminCommand command);
    }
}