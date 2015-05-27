using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class WordController : AbstractApiController
    {
        public HttpResponseMessage Get(string wordId)
        {
            if (wordId == null)
                ModelState.AddModelError("wordId", "Not specified");

            if (ModelState.IsValid == false)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));

            var word = ExecuteQuery(new GetWordQuery(wordId));
            return Request.CreateResponse(HttpStatusCode.OK, word);
        }

        public HttpResponseMessage GetAll(string wordListId)
        {
            if (wordListId == null)
                ModelState.AddModelError("wordListId", "Not specified");

            if (ModelState.IsValid == false)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));

            var words = ExecuteQuery(new GetWordsQuery(wordListId));
            return Request.CreateResponse(HttpStatusCode.OK, words);
        }

        public HttpResponseMessage Post(PostWordRequest request)
        {
            if (request == null)
                ModelState.AddModelError("request", "Not specified");

            if (ModelState.IsValid == false)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));

            Debug.Assert(request != null, "request != null");
            var wordList = ExecuteQuery(new GetWordListQuery(request.WordListId));
            var wordId = ExecuteCommand(new CreateWordCommand(request.Text, request.Definition, wordList));
            return Request.CreateResponse(HttpStatusCode.Created, new { wordId });
        }

        public HttpResponseMessage Put(PutWordRequest request)
        {
            if (request == null)
                ModelState.AddModelError("request", "Not specified");

            if (ModelState.IsValid == false)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));

            Debug.Assert(request != null, "request != null");
            ExecuteCommand(new UpdateWordCommand(request.WordId, request.Text, request.Definition));
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        public class PutWordRequest
        {
            public PutWordRequest(string wordId, string text, string definition)
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

        public class PostWordRequest
        {
            public PostWordRequest(string text, string definition, string wordListId)
            {
                WordListId = wordListId;
                Definition = definition;
                Text = text;
            }

            [Required, MaxLength(1024)]
            public string Text { get; private set; }

            [Required, MaxLength(1024)]
            public string Definition { get; private set; }

            [Required]
            public string WordListId { get; private set; }
        }
    }
}