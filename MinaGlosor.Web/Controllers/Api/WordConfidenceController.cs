using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Controllers.Api
{
    public class WordConfidenceController : AbstractApiController
    {
        public HttpResponseMessage Post(WordConfidenceRequest request)
        {
            var confidenceLevel = ConfidenceLevel.Unknown;
            if (request == null)
                ModelState.AddModelError("request", "Not specified");
            else
            {
                if (!Enum.TryParse(request.ConfidenceLevel, out confidenceLevel))
                    ModelState.AddModelError("confidenceLevel", "Unknown confidence level");
            }

            if (ModelState.IsValid == false)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));

            Debug.Assert(request != null, "request != null");
            var result = ExecuteCommand(new UpdateWordConfidenceCommand(request.PracticeSessionId, request.PracticeWordId, confidenceLevel));
            return Request.CreateResponse(HttpStatusCode.Created, result);
        }

        public class WordConfidenceRequest
        {
            public WordConfidenceRequest(string practiceSessionId, string practiceWordId, string confidenceLevel)
            {
                PracticeSessionId = practiceSessionId;
                ConfidenceLevel = confidenceLevel;
                PracticeWordId = practiceWordId;
            }

            [Required]
            public string PracticeSessionId { get; private set; }

            [Required]
            public string PracticeWordId { get; private set; }

            [Required]
            public string ConfidenceLevel { get; private set; }
        }
    }
}