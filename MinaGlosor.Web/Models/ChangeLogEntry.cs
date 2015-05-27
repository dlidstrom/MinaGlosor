using System;
using Raven.Abstractions;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class ChangeLogEntry
    {
        public ChangeLogEntry(string userId, string email, Guid correlationId, Type commandType, string commandAsJson)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (email == null) throw new ArgumentNullException("email");
            if (commandAsJson == null) throw new ArgumentNullException("commandAsJson");
            UserId = userId;
            Email = email;
            CorrelationId = correlationId;
            CommandType = commandType;
            CommandAsJson = commandAsJson;
            CreatedDate = SystemTime.UtcNow;
        }

        [JsonConstructor]
        private ChangeLogEntry()
        {
        }

        public string Id { get; private set; }

        public string UserId { get; private set; }

        public string Email { get; private set; }

        public Guid CorrelationId { get; private set; }

        public Type CommandType { get; private set; }

        public string CommandAsJson { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}