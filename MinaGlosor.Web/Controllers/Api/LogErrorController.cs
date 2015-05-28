using System;
using System.Web.Http;
using Elmah;

namespace MinaGlosor.Web.Controllers.Api
{
    public class LogErrorController : ApiController
    {
        public IHttpActionResult Post(JavaScriptExceptionMessage message)
        {
            if (message != null)
            {
                ErrorSignal.FromCurrentContext().Raise(new JavaScriptException(message.Message ?? "<no message>"));
            }

            return Ok();
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