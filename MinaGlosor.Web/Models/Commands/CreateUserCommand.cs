using System;
using MinaGlosor.Web.Infrastructure;
using Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateUserCommand : ICommand<string>
    {
        public CreateUserCommand(string email, string password, string username, UserRole userRole)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (password == null) throw new ArgumentNullException("password");
            if (username == null) throw new ArgumentNullException("username");

            Email = email;
            Password = password;
            UserRole = userRole;
            Username = username;
        }

        public string Email { get; private set; }

        [JsonIgnore]
        public string Password { get; private set; }

        public UserRole UserRole { get; private set; }

        public string Username { get; private set; }
    }
}