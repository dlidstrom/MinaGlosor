using System.Net;
using System.Net.Http;
using System.Web.Http;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers.Api
{
    public class PracticeWordController : AbstractApiController
    {
        public HttpResponseMessage GetNext(string practiceSessionId)
        {
            if (practiceSessionId == null)
                ModelState.AddModelError("practiceSessionId", "Not specified");

            if (ExecuteQuery(new IsPracticeSessionFinishedQuery(practiceSessionId)))
            {
                TracingLogger.Warning(
                    EventIds.Warning_Transient_4XXX.Web_PracticeSession_Finished_4001,
                    "Can not practice a finished practice session");
                ModelState.AddModelError("practiceSessionId", "Practice session is finished");
            }

            if (ModelState.IsValid == false)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));

            var practiceWord = ExecuteQuery(new GetNextPracticeWordQuery(practiceSessionId, CurrentUser.Id));
            ExecuteCommand(new UpdateLastPickedDateCommand(practiceSessionId, practiceWord.PracticeWordId));
            return Request.CreateResponse(HttpStatusCode.OK, practiceWord);
        }

        public HttpResponseMessage GetById(string practiceSessionId, string practiceWordId)
        {
            if (practiceSessionId == null)
                ModelState.AddModelError("practiceSessionId", "Not specified");

            if (practiceWordId == null)
                ModelState.AddModelError("practiceWordId", "Not specified");

            if (ExecuteQuery(new IsPracticeSessionFinishedQuery(practiceSessionId)))
            {
                TracingLogger.Warning(
                    EventIds.Warning_Transient_4XXX.Web_PracticeSession_Finished_4001,
                    "Can not practice a finished practice session");
                ModelState.AddModelError("practiceSessionId", "Practice session is finished");
            }

            if (ModelState.IsValid == false)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));

            var practiceWord = ExecuteQuery(new GetPracticeWordQuery(practiceSessionId, practiceWordId, CurrentUser.Id));
            return Request.CreateResponse(HttpStatusCode.OK, practiceWord);
        }
    }
}