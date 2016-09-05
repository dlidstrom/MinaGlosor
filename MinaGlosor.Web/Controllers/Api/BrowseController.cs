using System.Web.Http;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class BrowseController : AbstractApiController
    {
        private const int ItemsPerPage = 50;

        public IHttpActionResult Get(int? page = 1)
        {
            var result = ExecuteQuery(new BrowseQuery(page.GetValueOrDefault(1), ItemsPerPage));
            return Ok(result);
        }
    }
}