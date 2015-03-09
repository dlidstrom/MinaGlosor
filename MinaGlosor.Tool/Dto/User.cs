using System;

namespace MinaGlosor.Tool.Dto
{
    public class User
    {
        public string Email { get; set; }

        public string HashedPassword { get; set; }

        public string Username { get; set; }

        public DateTime CreatedDate { get; set; }

        public string RequestUsername { get; set; }

        public string RequestPassword { get; set; }

        public override string ToString()
        {
            var s = string.Format(
                "Email: {0}, Username: {1}, HashedPassword: {2}, CreatedDate: {3}",
                Email,
                Username,
                HashedPassword,
                CreatedDate);
            return s;
        }
    }
}