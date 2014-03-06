using System.Data.Entity;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Queries
{
    public class HasAdminUserQuery : IQuery<bool>
    {
        public async Task<bool> ExecuteAsync(IDbContext context)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.Role == UserRole.Admin) != null;
        }
    }
}