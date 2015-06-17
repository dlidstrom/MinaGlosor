using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class UserRegisteredEvent : ModelEvent
    {
        public UserRegisteredEvent(string id, string email, string hashedPassword, string username, UserRole userRole)
            : base(id)
        {
            Email = email;
            HashedPassword = hashedPassword;
            Username = username;
            UserRole = userRole;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private UserRegisteredEvent()
#pragma warning restore 612, 618
        {
        }

        public string Email { get; private set; }

        public string HashedPassword { get; private set; }

        public string Username { get; private set; }

        public UserRole UserRole { get; private set; }
    }
}