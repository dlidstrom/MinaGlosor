using System;
using System.ComponentModel.DataAnnotations;

namespace MinaGlosor.Web.Data.Models
{
    public class User
    {
        private readonly string password;

        public User(string email, string password)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (password == null) throw new ArgumentNullException("password");
            Email = email;
            Role = UserRole.Basic;
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        }

        public User(string email, string password, UserRole userRole)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (password == null) throw new ArgumentNullException("password");
            Email = email;
            Role = userRole;
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        }

        private User()
        {
        }

        public int Id { get; set; }

        [Required, MaxLength(320)]
        public string Email { get; private set; }

        [Required, MaxLength(120)]
        public string HashedPassword { get; private set; }

        public UserRole Role { get; private set; }

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