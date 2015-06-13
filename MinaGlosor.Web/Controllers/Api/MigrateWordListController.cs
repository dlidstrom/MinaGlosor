using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Indexes;

namespace MinaGlosor.Web.Controllers.Api
{
    public class MigrateWordListController : MigrationController
    {
        public IHttpActionResult Post(MigratedWordListRequest request)
        {
            return AsAdmin(request, session =>
            {
                var wordListOwner = session.Query<User, UserIndex>().SingleOrDefault(x => x.Email == request.OwnerEmail);
                if (wordListOwner == null)
                    return BadRequest(request.OwnerEmail + " not found");

                // check for existing wordlist
                var existingWordList = session.Query<WordList, WordListIndex>()
                                              .SingleOrDefault(x => x.Name == request.Name && x.OwnerId == wordListOwner.Id);
                if (existingWordList != null)
                    return Ok();

                // store wordlist
                session.Store(new WordList(KeyGeneratorBase.Generate<WordList>(session), request.Name, wordListOwner.Id));

                return StatusCode(HttpStatusCode.Created);
            });
        }

        public class MigratedWordListRequest : MigrationRequest
        {
            public MigratedWordListRequest(
                string requestUsername,
                string requestPassword,
                string ownerEmail,
                string name)
                : base(requestUsername, requestPassword)
            {
                OwnerEmail = ownerEmail;
                Name = name;
            }

            [Required]
            public string OwnerEmail { get; private set; }

            [Required]
            public string Name { get; private set; }
        }
    }
}