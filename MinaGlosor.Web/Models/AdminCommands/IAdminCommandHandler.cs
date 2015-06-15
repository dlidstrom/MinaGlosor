namespace MinaGlosor.Web.Models.AdminCommands
{
    public interface IAdminCommandHandler<in TCommand>
        where TCommand : IAdminCommand
    {
        void Run(TCommand command);
    }
}