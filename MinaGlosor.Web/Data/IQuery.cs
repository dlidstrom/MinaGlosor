using System.Threading.Tasks;

namespace MinaGlosor.Web.Data
{
    public interface IQuery<TResult>
    {
        Task<TResult> ExecuteAsync(IDbContext session);
    }
}