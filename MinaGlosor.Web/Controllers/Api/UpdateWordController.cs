using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Controllers.Api
{
    public class UpdateWordController : AbstractApiController
    {
        public HttpResponseMessage Post(PostWordRequest request)
        {
            if (request == null)
                ModelState.AddModelError("request", "Not specified");

            if (ModelState.IsValid == false)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));

            Debug.Assert(request != null, "request != null");
            ExecuteCommand(new UpdateWordCommand(request.WordId, request.Text, request.Definition));
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        public class PostWordRequest
        {
            public PostWordRequest(string wordId, string text, string definition)
            {
                Definition = definition;
                Text = text;
                WordId = wordId;
            }

            [Required]
            public string WordId { get; private set; }

            [Required, MaxLength(1024)]
            public string Text { get; private set; }

            [Required, MaxLength(1024)]
            public string Definition { get; private set; }
        }
    }
}