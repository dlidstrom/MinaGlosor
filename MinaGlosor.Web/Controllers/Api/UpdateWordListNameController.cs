using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Web.Http;
using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Controllers.Api
{
    public class UpdateWordListNameController : AbstractApiController
    {
        public IHttpActionResult Post(UpdateWordListNameRequest request)
        {
            if (request == null) ModelState.AddModelError("Request", "Invalid request");
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            Debug.Assert(request != null, "request != null");
            ExecuteCommand(new UpdateWordListNameCommand(request.WordListId, request.WordListName));
            return Ok();
        }

        public class UpdateWordListNameRequest
        {
            public UpdateWordListNameRequest(string wordListId, string wordListName)
            {
                WordListId = wordListId;
                WordListName = wordListName;
            }

            [Required]
            public string WordListId { get; private set; }

            [Required, MaxLength(1024)]
            public string WordListName { get; private set; }
        }
    }
}