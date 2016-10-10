using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Controllers.Api
{
    public class PracticeSessionController : AbstractApiController
    {
        public HttpResponseMessage Post(PostPracticeSessionRequest request)
        {
            if (request == null)
                ModelState.AddModelError("request", "Not specified");

            if (ModelState.IsValid == false)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));

            Debug.Assert(request != null, "request != null");
            Debug.Assert(request.PracticeMode != null, "request.PracticeMode != null");
            var result = ExecuteCommand(new CreatePracticeSessionCommand(request.WordListId, CurrentUser, request.PracticeMode.Value));
            return Request.CreateResponse(HttpStatusCode.Created, result);
        }

        public class PostPracticeSessionRequest
        {
            public PostPracticeSessionRequest(string wordListId, PracticeMode? practiceMode)
            {
                WordListId = wordListId;
                PracticeMode = practiceMode;
            }

            [Required]
            public string WordListId { get; private set; }

            [Required]
            public PracticeMode? PracticeMode { get; set; }
        }
    }
}