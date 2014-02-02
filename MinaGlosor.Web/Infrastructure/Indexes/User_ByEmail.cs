using System.Linq;
using MinaGlosor.Web.Models;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Infrastructure.Indexes
{
    public class User_ByEmail : AbstractIndexCreationTask<User>
    {
        public User_ByEmail()
        {
            Map = users => from user in users
                           select new
                               {
                                   user.Email,
                                   user.ActivationKey
                               };
        }
    }
}