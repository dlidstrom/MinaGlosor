using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Controllers.Api
{
    public class MigrateWordController : AbstractApiController
    {
        public HttpResponseMessage Post(MigrateWordRequest request)
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

            var wordList = session.Query<WordList, WordListIndex>()
                                  .SingleOrDefault(x => x.OwnerId == wordListOwner.Id && x.Name == request.WordListName);
            if (wordList == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, request.WordListName + " not found");

            // check for existing word
            var existingWord = session.Query<Word, WordIndex>()
                                      .SingleOrDefault(x => x.WordListId == wordList.Id && x.Text == request.Text && x.Definition == request.Definition);
            if (existingWord != null)
                return Request.CreateResponse(HttpStatusCode.OK);

            Debug.Assert(request.CreatedDate != null, "request.CreatedDate != null");
            var word = Word.CreateFromMigration(
                "Words/1",
                request.Text,
                request.Definition,
                request.CreatedDate.Value,
                wordList.Id,
                Guid.NewGuid(),
                null);
            session.Store(word);

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        public class MigrateWordRequest
        {
            public MigrateWordRequest(
                string requestUsername,
                string requestPassword,
                string ownerEmail,
                string wordListName,
                string text,
                string definition,
                DateTime? createdDate)
            {
                Definition = definition;
                Text = text;
                RequestUsername = requestUsername;
                RequestPassword = requestPassword;
                OwnerEmail = ownerEmail;
                WordListName = wordListName;
                CreatedDate = createdDate;
            }

            [Required]
            public string RequestUsername { get; private set; }

            [Required]
            public string RequestPassword { get; private set; }

            [Required]
            public string OwnerEmail { get; private set; }

            [Required]
            public string WordListName { get; private set; }

            [Required]
            public string Text { get; private set; }

            [Required]
            public string Definition { get; private set; }

            [Required]
            public DateTime? CreatedDate { get; private set; }
        }
    }
}