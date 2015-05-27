using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Controllers.Api
{
    public class MigrateUserController : AbstractApiController
    {
        public HttpResponseMessage Post(MigratedUserRequest request)
        {
            if (request == null) ModelState.AddModelError("request", "Invalid request");
            if (ModelState.IsValid == false)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ModelState, true));
            }

            var session = GetDocumentSession();

            // validate credentials
            var runAsQuery = from user in session.Query<User, UserIndex>()
                             where user.Email == request.RequestUsername
                             select user;
            var runAs = runAsQuery.FirstOrDefault();
            Debug.Assert(request != null, "request != null");
            if (runAs == null || runAs.ValidatePassword(request.RequestPassword) == false)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid credentials");
            }

            // make sure user does not exist
            var existsQuery = from user in session.Query<User, UserIndex>()
                              where user.Email == request.Email
                              select user;
            var existingUser = existsQuery.FirstOrDefault();
            if (existingUser != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            Debug.Assert(request.CreatedDate != null, "request.CreatedDate != null");
            var newUser = Models.User.CreateFromMigration(
                KeyGeneratorBase.Generate<User>(session),
                request.CreatedDate.Value,
                request.Email,
                request.HashedPassword,
                request.Username);
            session.Store(newUser);

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        public class MigratedUserRequest
        {
            public MigratedUserRequest(
                string requestUsername,
                string requestPassword,
                DateTime? createdDate,
                string email,
                string hashedPassword,
                string username)
            {
                RequestUsername = requestUsername;
                RequestPassword = requestPassword;
                CreatedDate = createdDate;
                Email = email;
                HashedPassword = hashedPassword;
                Username = username;
            }

            [Required]
            public string RequestUsername { get; private set; }

            [Required]
            public string RequestPassword { get; private set; }

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