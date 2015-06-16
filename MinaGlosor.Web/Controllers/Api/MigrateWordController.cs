using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Indexes;

namespace MinaGlosor.Web.Controllers.Api
{
    // TODO Change to admin command.
    public class MigrateWordController : MigrationController
    {
        public IHttpActionResult Post(MigrateWordRequest request)
        {
            return AsAdmin(request, session =>
            {
                var wordListOwner = session.Query<User, UserIndex>().SingleOrDefault(x => x.Email == request.OwnerEmail);
                if (wordListOwner == null)
                    return BadRequest(request.OwnerEmail + " not found");

                var wordList = session.Query<WordList, WordListIndex>()
                                      .SingleOrDefault(x => x.OwnerId == wordListOwner.Id && x.Name == request.WordListName);
                if (wordList == null)
                    return BadRequest(request.WordListName + " not found");

                // check for existing word
                var existingWord = session.Query<Word, WordIndex>()
                                          .SingleOrDefault(x => x.WordListId == wordList.Id && x.Text == request.Text && x.Definition == request.Definition);
                if (existingWord != null)
                    return Ok();

                Debug.Assert(request.CreatedDate != null, "request.CreatedDate != null");
                var generator = new KeyGenerator<Word>(session);
                var word = Word.CreateFromMigration(
                    generator.Generate(),
                    request.Text,
                    request.Definition,
                    request.CreatedDate.Value,
                    wordList.Id);
                session.Store(word);

                return StatusCode(HttpStatusCode.Created);
            });
        }

        public class MigrateWordRequest : MigrationRequest
        {
            public MigrateWordRequest(
                string requestUsername,
                string requestPassword,
                string ownerEmail,
                string wordListName,
                string text,
                string definition,
                DateTime? createdDate)
                : base(requestUsername, requestPassword)
            {
                Definition = definition;
                Text = text;
                OwnerEmail = ownerEmail;
                WordListName = wordListName;
                CreatedDate = createdDate;
            }

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