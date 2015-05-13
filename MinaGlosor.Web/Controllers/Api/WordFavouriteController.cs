using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Web.Http;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class WordFavouriteController : AbstractApiController
    {
        public IHttpActionResult Post(RegisterFavouriteRequest request)
        {
            if (request == null)
            {
                ModelState.AddModelError("request", "Invalid request");
            }

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            Debug.Assert(request != null, "request != null");
            var result = ExecuteCommand(new ToggleWordFavouriteCommand(request.WordId, CurrentUser.Id));
            return Ok(result);
        }

        public IHttpActionResult GetAll()
        {
            var result = ExecuteQuery(new GetWordFavouritesQuery(CurrentUser.Id));
            return Ok(result);
        }

        public class RegisterFavouriteRequest
        {
            public RegisterFavouriteRequest(string wordId)
            {
                WordId = wordId;
            }

            [Required]
            public string WordId { get; private set; }
        }
    }
}