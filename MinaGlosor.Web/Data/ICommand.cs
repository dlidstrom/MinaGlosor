namespace MinaGlosor.Web.Data
{
    public interface ICommand
    {
        void Execute(IDbContext session);
    }
}