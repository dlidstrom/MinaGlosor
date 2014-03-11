using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Elmah;

namespace MinaGlosor.Web.Controllers.Api
{
    public class LogErrorController : ApiController
    {
        public HttpResponseMessage Post(JavaScriptExceptionMessage message)
        {
            ErrorSignal.FromCurrentContext().Raise(new JavaScriptException(message.Message));
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public class JavaScriptExceptionMessage
        {
            public string Message { get; set; }
        }

        private class JavaScriptException : Exception
        {
            public JavaScriptException(string message)
                : base(message)
            {
            }
        }
    }
}