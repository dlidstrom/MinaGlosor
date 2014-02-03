using System.Linq;
using MinaGlosor.Web.Data.Models;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Infrastructure.Indexes
{
    public class CreateAccountRequestIndex : AbstractIndexCreationTask<CreateAccountRequest>
    {
        public CreateAccountRequestIndex()
        {
            Map = requests => from request in requests
                              select new
                                  {
                                      request.Email,
                                      request.ActivationCode
                                  };
        }
    }
}