using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class WordListProgressController : AbstractApiController
    {
        public HttpResponseMessage GetAll()
        {
            var wordLists = ExecuteQuery(new GetWordListProgressesQuery(CurrentUser.Id));
            return Request.CreateResponse(HttpStatusCode.OK, wordLists);
        }
    }
}