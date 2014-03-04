using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MinaGlosor.Web.Data.Commands;
using MinaGlosor.Web.Data.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class WordController : ApiControllerBase
    {
        public async Task<HttpResponseMessage> Get(int wordListId)
        {
            var words = await ExecuteQueryAsync(new GetWordsQuery(wordListId));
            return Request.CreateResponse(HttpStatusCode.OK, words);
        }

        public async Task<HttpResponseMessage> Post(NewWordRequest request)
        {
            if (ModelState.IsValid == false)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));

            await ExecuteCommandAsync(new CreateWordCommand(request.WordListId, request.Word, request.Definition));
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        public class NewWordRequest
        {
            public int WordListId { get; set; }

            [Required, MaxLength(1024)]
            public string Word { get; set; }

            [Required, MaxLength(1024)]
            public string Definition { get; set; }
        }
    }
}