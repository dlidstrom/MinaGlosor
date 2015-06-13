using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Controllers.Api
{
    public abstract class MigrationController : AbstractApiController
    {
        protected IHttpActionResult AsAdmin(MigrationRequest request, Func<IDocumentSession, IHttpActionResult> func)
        {
            if (request == null) ModelState.AddModelError("request", "Invalid request");
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
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
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            var result = func.Invoke(session);
            return result;
        }

        public abstract class MigrationRequest
        {
            protected MigrationRequest(string requestUsername, string requestPassword)
            {
                RequestUsername = requestUsername;
                RequestPassword = requestPassword;
            }

            [Required]
            public string RequestUsername { get; private set; }

            [Required]
            public string RequestPassword { get; private set; }
        }
    }
}