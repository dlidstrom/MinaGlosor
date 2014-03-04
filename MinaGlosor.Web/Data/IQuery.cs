namespace MinaGlosor.Web.Data
{
    public interface IQuery<out TResult>
    {
        TResult Execute(IDbContext session);
    }
}