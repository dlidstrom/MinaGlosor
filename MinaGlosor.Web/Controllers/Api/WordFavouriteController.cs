using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Web.Http;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class WordFavouriteController : AbstractApiController
    {
        private const int ItemsPerPage = 50;

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
            Debug.Assert(request.IsFavourite != null, "request.IsFavourite != null");
            var result = ExecuteCommand(new ToggleWordFavouriteCommand(request.WordId, request.IsFavourite.Value, CurrentUser.Id));
            return Ok(result);
        }

        public IHttpActionResult GetAll(int? page)
        {
            var result = ExecuteQuery(new GetWordFavouritesQuery(CurrentUser.Id, page.GetValueOrDefault(1), ItemsPerPage));
            return Ok(result);
        }

        public class RegisterFavouriteRequest
        {
            public RegisterFavouriteRequest(string wordId, bool? isFavourite)
            {
                WordId = wordId;
                IsFavourite = isFavourite;
            }

            [Required]
            public string WordId { get; private set; }

            [Required]
            public bool? IsFavourite { get; private set; }
        }
    }
}