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
            HashedPassword = ComputeHashedPassword(password);
        }

        public User(string email, string password, UserRole userRole)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (password == null) throw new ArgumentNullException("password");
            Email = email;
            Role = userRole;
            HashedPassword = ComputeHashedPassword(password);
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
            return HashedPassword == ComputeHashedPassword(somePassword);
        }

        public void SetRole(UserRole role)
        {
            Role = role;
        }

        private static string ComputeHashedPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}