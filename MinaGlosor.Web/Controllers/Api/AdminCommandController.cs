using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.AdminCommands;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Indexes;
using Newtonsoft.Json;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Controllers.Api
{
    public class AdminCommandController : AbstractApiController
    {
        public IHttpActionResult Post(AdminCommandRequest request)
        {
            IAdminCommand command = null;
            if (request == null)
            {
                ModelState.AddModelError("request", "Invalid request");
            }
            else
            {
                try
                {
                    var serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    command = JsonConvert.DeserializeObject<IAdminCommand>(request.CommandJson, serializerSettings);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("request", "Not a valid command");
                }
            }

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            Debug.Assert(command != null, "command != null");

            var session = GetDocumentSession();

            // validate credentials
            var runAsQuery = from user in session.Query<User, UserIndex>()
                             where user.Email == command.RequestUsername
                             select user;
            var runAs = runAsQuery.FirstOrDefault();
            if (runAs == null || runAs.ValidatePassword(command.RequestPassword) == false)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            var result = ExecuteCommand(new RunAdminCommand(Kernel, command));
            return Ok(result);
        }

        public class AdminCommandRequest
        {
            public AdminCommandRequest(string commandJson)
            {
                CommandJson = commandJson;
            }

            public string CommandJson { get; private set; }
        }
    }
}