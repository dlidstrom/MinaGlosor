using System.Net;
using System.Net.Http;
using System.Web.Http;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class UnfinishedPracticeSessionController : AbstractApiController
    {
        public HttpResponseMessage Get(string wordListId)
        {
            if (wordListId == null)
                ModelState.AddModelError("wordListId", "Missing wordListId");

            if (ModelState.IsValid == false)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));

            var result = ExecuteQuery(new GetUnfinishedPracticeSessionsQuery(wordListId, CurrentUser.Id));
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}