using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.AdminCommands;
using MinaGlosor.Web.Models.Indexes;
using Newtonsoft.Json;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Controllers.Api
{
    public class MigrateAdminUserController : MigrationController
    {
        public IHttpActionResult Post(string commandJson)
        {
            return AsAdmin(commandJson, session =>
            {
                var adminUsersQuery = from user in session.Query<User, UserIndex>().Customize(x => x.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(30)))
                                      where user.Role == UserRole.Admin
                                      select user;
                var adminUsers = adminUsersQuery.ToArray();
                var websiteConfig = session.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
                if (websiteConfig == null)
                {
                    websiteConfig = new WebsiteConfig();
                    session.Store(websiteConfig);
                }

                var migratedUsers = new List<string>();
                foreach (var adminUser in adminUsers)
                {
                    if (websiteConfig.IsAdminUser(adminUser.Id) == false)
                    {
                        websiteConfig.AddAdminUser(adminUser.Id);
                        migratedUsers.Add(adminUser.Id);
                    }
                }

                var result = new
                {
                    MigratedUsers = migratedUsers.ToArray()
                };
                return Ok(result);
            });
        }

        protected IHttpActionResult AsAdmin(string request, Func<IDocumentSession, IHttpActionResult> func)
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
                    command = JsonConvert.DeserializeObject<IAdminCommand>(request);
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

            var result = func.Invoke(session);
            return result;
        }
    }
}