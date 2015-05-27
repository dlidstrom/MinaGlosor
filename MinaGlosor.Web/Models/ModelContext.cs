using System;
using System.Runtime.Remoting.Messaging;

namespace MinaGlosor.Web.Models
{
    public class ModelContext : IDisposable
    {
        private readonly Guid oldCorrelationId;
        private readonly Guid oldCausationId;

        public ModelContext(Guid correlationId, Guid? causationId = null)
        {
            oldCorrelationId = CorrelationId;
            oldCausationId = CausationId;
            CorrelationId = correlationId;
            CausationId = causationId.GetValueOrDefault();
        }

        public static Guid CorrelationId
        {
            get
            {
                var correlationId = CallContext.GetData("CorrelationId");
                if (correlationId is Guid) return (Guid)correlationId;
                return default(Guid);
            }

            set
            {
                CallContext.SetData("CorrelationId", value);
            }
        }

        public static Guid CausationId
        {
            get
            {
                var causationId = CallContext.GetData("CausationId");
                if (causationId is Guid) return (Guid)causationId;
                return default(Guid);
            }

            set
            {
                CallContext.SetData("CausationId", value);
            }
        }

        public void Dispose()
        {
            CorrelationId = oldCorrelationId;
            CausationId = oldCausationId;
        }
    }
}