using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Web.Http;
using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Controllers.Api
{
    public class PublishWordListController : AbstractApiController
    {
        public IHttpActionResult Post(PublisWordListRequest request)
        {
            if (request == null) ModelState.AddModelError("Request", "Invalid request");
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            Debug.Assert(request != null, "request != null");
            Debug.Assert(request.Publish != null, "request.Publish != null");
            ExecuteCommand(new PublishWorListCommand(request.WordListId, request.Publish.Value));
            return Ok();
        }

        public class PublisWordListRequest
        {
            public PublisWordListRequest(string wordListId, bool? publish)
            {
                WordListId = wordListId;
                Publish = publish;
            }

            [Required]
            public string WordListId { get; private set; }

            [Required]
            public bool? Publish { get; private set; }
        }
    }
}