using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Controllers.Api
{
    public class MigrateUserController : MigrationController
    {
        public IHttpActionResult Post(MigratedUserRequest request)
        {
            return AsAdmin(request, session =>
            {
                // make sure user does not exist
                var existsQuery = from user in session.Query<User, UserIndex>()
                                  where user.Email == request.Email
                                  select user;
                var existingUser = existsQuery.FirstOrDefault();
                if (existingUser != null)
                {
                    return Ok();
                }

                Debug.Assert(request.CreatedDate != null, "request.CreatedDate != null");
                var newUser = Models.User.CreateFromMigration(
                    KeyGeneratorBase.Generate<User>(session),
                    request.CreatedDate.Value,
                    request.Email,
                    request.HashedPassword,
                    request.Username);
                session.Store(newUser);

                return StatusCode(HttpStatusCode.Created);
            });
        }

        public class MigratedUserRequest : MigrationRequest
        {
            public MigratedUserRequest(
                string requestUsername,
                string requestPassword,
                DateTime? createdDate,
                string email,
                string hashedPassword,
                string username)
                : base(requestUsername, requestPassword)
            {
                CreatedDate = createdDate;
                Email = email;
                HashedPassword = hashedPassword;
                Username = username;
            }

            [Required]
            public DateTime? CreatedDate { get; private set; }

            [Required]
            public string Email { get; private set; }

            [Required]
            public string HashedPassword { get; private set; }

            [Required]
            public string Username { get; private set; }
        }
    }
}