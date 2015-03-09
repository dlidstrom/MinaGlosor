using System.Linq;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class CreateAccountRequestIndex : AbstractIndexCreationTask<CreateAccountRequest>
    {
        public CreateAccountRequestIndex()
        {
            Map = requests => from request in requests
                              select new
                              {
                                  request.ActivationCode
                              };
        }
    }
}