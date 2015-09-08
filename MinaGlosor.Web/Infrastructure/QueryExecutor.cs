using System;
using System.Diagnostics;
using System.Security;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models;

namespace MinaGlosor.Web.Infrastructure
{
    public class QueryExecutor
    {
        private readonly IKernel kernel;

        public QueryExecutor(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public TResult ExecuteQuery<TResult>(IQuery<TResult> query, User user)
        {
            var handlerType = typeof(QueryHandlerBase<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handleMethod = handlerType.GetMethod("Handle");
            try
            {
                using (kernel.BeginScope())
                using (new ActivityScope(
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteQueryStart_3012,
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteQueryStop_3013,
                    query.GetType().Name))
                using (new ModelContext(Trace.CorrelationManager.ActivityId))
                {
                    var queryAsJson = query.ToJson();
                    TracingLogger.Information(EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteQueryLog_3014, queryAsJson);
                    IQueryHandler handler = null;
                    try
                    {
                        handler = (IQueryHandler)kernel.Resolve(handlerType);
                        if (user != null && handler.CanExecute(user) == false)
                        {
                            var message = string.Format("Operation not allowed: {0} {1}", query.GetType().Name, user.Username);
                            throw new SecurityException(message);
                        }

                        var result = (TResult)handleMethod.Invoke(handler, new[] { (object)query });
                        TracingLogger.Information(EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteQueryResult_3015, result.ToJson());
                        return result;
                    }
                    finally
                    {
                        if (handler != null) kernel.ReleaseComponent(handler);
                    }
                }
            }
            catch (ComponentNotFoundException ex)
            {
                var message = string.Format(
                    "No handler found for {0}. Implement QueryHandlerBase<{0}>",
                    query.GetType().Name);
                throw new ApplicationException(message, ex);
            }
        }
    }
}