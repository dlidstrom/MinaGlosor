using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;

namespace MinaGlosor.Web.Infrastructure.Tracing
{
    public static class TracingLogger
    {
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Ok here.")]
        public static Action<TraceEventType, int, object[]> DoTraceData = (eventType, id, args) => TraceSource.TraceData(eventType, id, args);

        private static readonly TraceSource TraceSource = new TraceSource("MinaGlosor");

        public static void Information(string message)
        {
            TraceSource.TraceEvent(TraceEventType.Information, 0, message);
        }

        [StringFormatMethod("format")]
        public static void Information(string format, params object[] args)
        {
            TraceSource.TraceEvent(TraceEventType.Information, 0, format, args);
        }

        public static void Information(int id, string message)
        {
            TraceData(TraceEventType.Information, id, message);
        }

        [StringFormatMethod("format")]
        public static void Information(int id, string format, params object[] args)
        {
            TraceData(TraceEventType.Information, id, string.Format(format, args));
        }

        public static void Start(int id, string message)
        {
            TraceData(TraceEventType.Start, id, message);
        }

        [StringFormatMethod("format")]
        public static void Start(int id, string format, params object[] args)
        {
            TraceData(TraceEventType.Start, id, string.Format(format, args));
        }

        public static void Stop(int id, string message)
        {
            TraceData(TraceEventType.Stop, id, message);
        }

        [StringFormatMethod("format")]
        public static void Stop(int id, string format, params object[] args)
        {
            TraceData(TraceEventType.Stop, id, string.Format(format, args));
        }

        public static void Warning(int id, string message)
        {
            TraceData(TraceEventType.Warning, id, message);
        }

        public static void Warning(int id, Exception exception)
        {
            var list = new List<object>
            {
                Trace.CorrelationManager.ActivityId,
                exception
            };
            list.AddRange((from object key in exception.Data.Keys let ob = exception.Data[key] select string.Format("{0}={1}", key, ob)));
            TraceSource.TraceData(TraceEventType.Warning, id, list.ToArray());
        }

        [StringFormatMethod("format")]
        public static void Warning(int id, string format, params object[] args)
        {
            TraceData(TraceEventType.Warning, id, string.Format(format, args));
        }

        public static void Error(int id, string message)
        {
            TraceData(TraceEventType.Error, id, message);
        }

        public static void Error(int id, Exception exception)
        {
            var list = new List<object>
            {
                Trace.CorrelationManager.ActivityId,
                exception
            };
            list.AddRange((from object key in exception.Data.Keys let ob = exception.Data[key] select string.Format("{0}={1}", key, ob)));
            TraceSource.TraceData(TraceEventType.Error, id, list.ToArray());
        }

        [StringFormatMethod("format")]
        public static void Error(int id, string format, params object[] args)
        {
            TraceData(TraceEventType.Error, id, string.Format(format, args));
        }

        public static void Critical(int id, string message)
        {
            TraceData(TraceEventType.Critical, id, message);
        }

        public static void Critical(int id, Exception exception)
        {
            var list = new List<object>
            {
                Trace.CorrelationManager.ActivityId,
                exception
            };
            list.AddRange((from object key in exception.Data.Keys let ob = exception.Data[key] select string.Format("{0}={1}", key, ob)));
            TraceSource.TraceData(TraceEventType.Critical, id, list.ToArray());
        }

        [StringFormatMethod("format")]
        public static void Critical(int id, string format, params object[] args)
        {
            TraceData(TraceEventType.Critical, id, string.Format(format, args));
        }

        private static void TraceData(TraceEventType traceEventType, int id, string message)
        {
            if (Trace.CorrelationManager.ActivityId != default(Guid))
            {
                DoTraceData(traceEventType, id, Params(Trace.CorrelationManager.ActivityId, message));
            }
            else
            {
                DoTraceData.Invoke(traceEventType, id, Params(message));
            }
        }

        private static object[] Params(params object[] args)
        {
            return args;
        }
    }
}