using System.Web.Http;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class BrowseController : AbstractApiController
    {
        private const int ItemsPerPage = 1;

        public IHttpActionResult Get(int? page)
        {
            var result = ExecuteQuery(new BrowseQuery(page.GetValueOrDefault(1), ItemsPerPage));
            return Ok(result);
        }
    }
}