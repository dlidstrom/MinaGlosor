using System.Linq;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class ResetPasswordRequestIndex : AbstractIndexCreationTask<ResetPasswordRequest>
    {
        public ResetPasswordRequestIndex()
        {
            Map = requests => from request in requests
                              select new
                                  {
                                      request.ActivationCode
                                  };
        }
    }
}