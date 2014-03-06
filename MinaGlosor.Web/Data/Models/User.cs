using System.ComponentModel.DataAnnotations;

namespace MinaGlosor.Web.Data.Models
{
    public class User
    {
        public User(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = UserRole.Basic;
            HashedPassword = ComputeHashedPassword(password);
        }

        private User()
        {
        }

        public int Id { get; set; }

        [Required, MaxLength(320)]
        public string Email { get; private set; }

        [Required, MaxLength(120)]
        public string FirstName { get; private set; }

        [Required, MaxLength(120)]
        public string LastName { get; private set; }

        [Required, MaxLength(50)]
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