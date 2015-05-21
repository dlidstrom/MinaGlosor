using System;
using Raven.Abstractions;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class ChangeLogEntry
    {
        public ChangeLogEntry(string userId, string email, Guid correlationId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (email == null) throw new ArgumentNullException("email");
            UserId = userId;
            Email = email;
            CorrelationId = correlationId;
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

        public DateTime CreatedDate { get; private set; }
    }
}