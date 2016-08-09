using System;
using System.Collections.Generic;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class BackgroundTask
    {
        [JsonConstructor]
        private BackgroundTask(Guid correlationId, Guid causationId, object body, DateTimeOffset nextTry)
        {
            CorrelationId = correlationId;
            CausationId = causationId;
            Body = body;
            Exceptions = new List<Exception>();
            NextTry = nextTry;
        }

        public string Id { get; private set; }

        public int Retries { get; private set; }

        public Guid CorrelationId { get; private set; }

        public Guid CausationId { get; private set; }

        public object Body { get; private set; }

        public bool IsFinished { get; private set; }

        public bool IsFailed { get; private set; }

        public DateTimeOffset NextTry { get; private set; }

        public List<Exception> Exceptions { get; private set; }

        public static BackgroundTask Create<TBody>(Guid correlationId, Guid causationId, TBody body, DateTimeOffset nextTry) where TBody : class
        {
            if (body == null) throw new ArgumentNullException("body");
            return new BackgroundTask(correlationId, causationId, body, nextTry);
        }

        public void MarkFinished()
        {
            IsFinished = true;
        }

        public void MarkFailed()
        {
            IsFailed = true;
        }

        public void UpdateNextTry(Exception exception)
        {
            Exceptions.Add(exception);

            Retries++;
            const int MaximumRetries = 5;
            if (Retries >= MaximumRetries)
            {
                MarkFailed();
            }
            else
            {
                NextTry = DateTimeOffset.UtcNow;
            }
        }

        public string GetInfo()
        {
            var info = string.Format("{0}{1}", Body.GetType(), Body);
            return info;
        }
    }
}