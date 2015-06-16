using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.AdminCommands;
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

            object handler = null;
            try
            {
                var handlerType = typeof(IAdminCommandHandler<>).MakeGenericType(command.GetType());
                handler = Kernel.Resolve(handlerType);
                var methodInfo = handlerType.GetMethod("Run");
                var result = methodInfo.Invoke(handler, new[] { (object)command });

                return Ok(result);
            }
            finally
            {
                Kernel.ReleaseComponent(handler);
            }
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