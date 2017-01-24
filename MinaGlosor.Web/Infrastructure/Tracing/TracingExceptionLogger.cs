using System.Web.Http.ExceptionHandling;

namespace MinaGlosor.Web.Infrastructure.Tracing
{
    public class TracingExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            TracingLogger.Error(EventIds.Error_Permanent_5XXX.Web_UnhandledException_5000, context.Exception);
        }
    }
}