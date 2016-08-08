using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class WordListController : AbstractApiController
    {
        public HttpResponseMessage GetAll()
        {
            var wordLists = ExecuteQuery(new GetWordListProgressesQuery(CurrentUser.Id));
            return Request.CreateResponse(HttpStatusCode.OK, wordLists);
        }

        public HttpResponseMessage GetById(string wordListId)
        {
            var wordList = ExecuteQuery(new GetWordListQuery(wordListId));
            return Request.CreateResponse(HttpStatusCode.OK, wordList);
        }

        public HttpResponseMessage Post(NewWordListRequest request)
        {
            if (request == null) ModelState.AddModelError("Request", "Invalid request");
            if (ModelState.IsValid == false)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));
            }

            Debug.Assert(request != null, "request != null");
            var result = ExecuteCommand(new CreateWordListCommand(request.Name, CurrentUser));
            return Request.CreateResponse(HttpStatusCode.Created, result);
        }

        public class NewWordListRequest
        {
            public NewWordListRequest(string name)
            {
                Name = name;
            }

            [Required, MaxLength(1024)]
            public string Name { get; private set; }
        }
    }
}