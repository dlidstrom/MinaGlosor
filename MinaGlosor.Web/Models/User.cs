using System;
using System.Text.RegularExpressions;
using Raven.Abstractions;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class User
    {
        public const string UsernamePattern = "^(?=.{4,20}$)(?![-])(?!.*[-]{2})[a-zA-Z0-9-]+(?<![-])$";

        public User(string email, string password, string username, UserRole userRole = UserRole.Basic)
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

            Email = email;
            Username = username;
            Role = userRole;
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            CreatedDate = SystemTime.UtcNow;
        }

        [JsonConstructor]
        private User(string email, string username)
        {
            Email = email;
            if (username == null)
            {
                username = email.Substring(0, email.IndexOf("@", StringComparison.Ordinal));
            }

            Username = Regex.Replace(username, @"(^-)|([^-a-zA-Z0-9])|(-$)", string.Empty);
        }

        private User()
        {
        }

        public string Id { get; private set; }

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

        public static User CreateFromMigration(DateTime createdDate, string email, string hashedPassword, string username)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (hashedPassword == null) throw new ArgumentNullException("hashedPassword");
            if (username == null) throw new ArgumentNullException("username");

            var user = new User
                {
                    CreatedDate = createdDate,
                    Email = email,
                    HashedPassword = hashedPassword,
                    Username = username,
                    Role = UserRole.Basic
                };
            return user;
        }

        public bool ValidatePassword(string somePassword)
        {
            return BCrypt.Net.BCrypt.Verify(somePassword, HashedPassword);
        }

        public void SetRole(UserRole role)
        {
            Role = role;
        }
    }
}