using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class WordListController : AbstractApiController
    {
        public HttpResponseMessage GetById(string wordListId)
        {
            var wordList = ExecuteQuery(new GetWordListQuery(WordList.ToId(wordListId)));
            return Request.CreateResponse(HttpStatusCode.OK, wordList);
        }

        // TODO Fixa alla s�na h�r (gamla sorten)
        public HttpResponseMessage Post(NewWordListRequest request)
        {
            if (request == null) ModelState.AddModelError("Request", "Invalid request");
            if (ModelState.IsValid == false)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));
            }

            Debug.Assert(request != null, "request != null");
            var result = ExecuteCommand(new CreateWordListCommand(request.Name, CurrentUser.Id));
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