using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Data.Commands;

namespace MinaGlosor.Web.Controllers.Api
{
    public class PracticeSessionController : ApiControllerBase
    {
        public HttpResponseMessage Post(CreatePracticeSessionRequest request)
        {
            ExecuteCommandAsync(new CreatePracticeSessionCommand(request.WordListId));
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        public class CreatePracticeSessionRequest
        {
            public int WordListId { get; set; }
        }
    }
}