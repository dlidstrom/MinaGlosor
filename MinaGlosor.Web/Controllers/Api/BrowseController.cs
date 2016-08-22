using System.Web.Http;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class BrowseController : AbstractApiController
    {
        public IHttpActionResult Get()
        {
            var result = ExecuteQuery(new BrowseQuery());
            return Ok(result);
        }
    }
}