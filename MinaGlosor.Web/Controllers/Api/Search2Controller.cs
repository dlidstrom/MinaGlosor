using System.Web.Http;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class Search2Controller : AbstractApiController
    {
        public IHttpActionResult Get(string q)
        {
            if (q == null)
            {
                ModelState.AddModelError("q", "Invalid request");
            }

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var result = ExecuteQuery(new SearchQuery(q, CurrentUser.Id));
            return Ok(result);
        }
    }
}