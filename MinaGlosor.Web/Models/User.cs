using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class User : DomainModel
    {
        public const string UsernamePattern = "^(?=.{4,20}$)(?![-])(?!.*[-]{2})[a-zA-Z0-9-]+(?<![-])$";

        public User(string id, string email, string password, string username, UserRole userRole = UserRole.Basic)
            : base(id)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (password == null) throw new ArgumentNullException("password");
            if (username == null) throw new ArgumentNullException("username");
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is empty");
            if (Regex.IsMatch(username, UsernamePattern) == false)
            {
                throw new ArgumentException(
                    "Username may be 4-20 characters, only contain alphanumeric characters or dashes, and cannot begin or end with a dash",
                    "username");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            Apply(new UserRegisteredEvent(id, email, hashedPassword, username, userRole));
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private User(string email, string username)
#pragma warning restore 612, 618
        {
            Email = email;
            if (username == null)
            {
                username = email.Substring(0, email.IndexOf("@", StringComparison.Ordinal));
            }

            Username = Regex.Replace(username, @"(^-)|([^-a-zA-Z0-9])|(-$)", string.Empty);
        }

#pragma warning disable 618

        private User()
#pragma warning restore 612, 618
        {
        }

        public string Email { get; private set; }

        public string Username { get; private set; }

        public string HashedPassword { get; private set; }

        public UserRole Role { get; private set; }

        public DateTime CreatedDate { get; private set; }

        [JsonIgnore]
        public bool IsAdmin
        {
            get { return Role == UserRole.Admin; }
        }

        public static string FromId(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            return userId.Substring(6);
        }

        public static User CreateFromMigration(string id, DateTime createdDate, string email, string hashedPassword, string username)
        {
            if (id == null) throw new ArgumentNullException("id");
            if (email == null) throw new ArgumentNullException("email");
            if (hashedPassword == null) throw new ArgumentNullException("hashedPassword");
            if (username == null) throw new ArgumentNullException("username");

            var user = new User();
            user.Apply(new UserRegisteredEvent(id, email, hashedPassword, username, UserRole.Basic));
            user.Apply(new SetUserCreatedEvent(id, createdDate));
            return user;
        }

        public bool ValidatePassword(string somePassword)
        {
            return BCrypt.Net.BCrypt.Verify(somePassword, HashedPassword);
        }

        public void SetRole(UserRole role)
        {
            Apply(new SetRoleEvent(Id, role));
        }

        public void SetPassword(string password)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            Apply(new SetPasswordEvent(Id, hashedPassword));
        }

        private void ApplyEvent(SetRoleEvent @event)
        {
            Role = @event.Role;
        }

        private void ApplyEvent(UserRegisteredEvent @event)
        {
            Email = @event.Email;
            HashedPassword = @event.HashedPassword;
            Username = @event.Username;
            Role = @event.UserRole;
            CreatedDate = @event.CreatedDateTime;
        }

        private void ApplyEvent(SetUserCreatedEvent @event)
        {
            CreatedDate = @event.CreatedDate;
        }

        private void ApplyEvent(SetPasswordEvent @event)
        {
            HashedPassword = @event.HashedPassword;
        }
    }
}