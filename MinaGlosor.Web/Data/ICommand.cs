using System.Threading.Tasks;

namespace MinaGlosor.Web.Data
{
    public interface ICommand
    {
        Task ExecuteAsync(IDbContext context);
    }
}