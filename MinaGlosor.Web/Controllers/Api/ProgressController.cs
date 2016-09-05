using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class ProgressController : AbstractApiController
    {
        private const int ItemsPerPage = 20;

        public HttpResponseMessage GetAll(int? page = 1)
        {
            var wordLists = ExecuteQuery(new GetProgressListQuery(CurrentUser.Id, page.GetValueOrDefault(1), ItemsPerPage));
            return Request.CreateResponse(HttpStatusCode.OK, wordLists);
        }
    }
}