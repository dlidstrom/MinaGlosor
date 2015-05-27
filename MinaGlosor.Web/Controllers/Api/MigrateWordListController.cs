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
    public class MigrateWordListController : AbstractApiController
    {
        public HttpResponseMessage Post(MigratedWordListRequest request)
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

            var wordListOwner = session.Query<User, UserIndex>().SingleOrDefault(x => x.Email == request.OwnerEmail);
            if (wordListOwner == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, request.OwnerEmail + " not found");

            // check for existing wordlist
            var existingWordList = session.Query<WordList, WordListIndex>()
                                          .SingleOrDefault(x => x.Name == request.Name && x.OwnerId == wordListOwner.Id);
            if (existingWordList != null)
                return Request.CreateResponse(HttpStatusCode.OK);

            // store wordlist
            session.Store(new WordList(KeyGeneratorBase.Generate<WordList>(session), request.Name, wordListOwner.Id));

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        public class MigratedWordListRequest
        {
            public MigratedWordListRequest(string requestUsername, string requestPassword, string ownerEmail, string name)
            {
                RequestUsername = requestUsername;
                RequestPassword = requestPassword;
                OwnerEmail = ownerEmail;
                Name = name;
            }

            [Required]
            public string RequestUsername { get; private set; }

            [Required]
            public string RequestPassword { get; private set; }

            [Required]
            public string OwnerEmail { get; private set; }

            [Required]
            public string Name { get; private set; }
        }
    }
}