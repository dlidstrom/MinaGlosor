using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MinaGlosor.Web.Data.Commands;
using MinaGlosor.Web.Data.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class WordListController : ApiControllerBase
    {
        public async Task<HttpResponseMessage> Get()
        {
            var query = new GetWordListsQuery(CurrentUser);
            var etag = ExecuteQueryForEtag(query);

            if (Request.Headers.IfNoneMatch.SingleOrDefault(x => x.Tag == etag) != null)
                return Request.CreateResponse(HttpStatusCode.NotModified);

            var wordLists = await ExecuteQueryAsync(query);
            var response = Request.CreateResponse(HttpStatusCode.OK, wordLists);
            //response.Headers.ETag = new EntityTagHeaderValue(etag);
            return response;
        }

        public HttpResponseMessage Get(int id)
        {
            var wordList = ExecuteQueryAsync(new GetWordListByIdQuery(id));
            if (wordList != null) return Request.CreateResponse(HttpStatusCode.OK, wordList);

            ModelState.AddModelError("id", string.Format("No word list found with id {0}", id));
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError(ModelState, true));
        }

        public async Task<HttpResponseMessage> Post(CreateWordListRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    new HttpError(ModelState, true));
            }

            await ExecuteCommandAsync(new CreateWordListCommand(request.WordListName, CurrentUser));
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        public class CreateWordListRequest
        {
            [Required, MaxLength(1024)]
            public string WordListName { get; set; }
        }
    }
}