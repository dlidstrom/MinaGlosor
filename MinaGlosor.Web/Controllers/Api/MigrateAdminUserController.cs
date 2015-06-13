using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Controllers.Api
{
    public class MigrateAdminUserController : MigrationController
    {
        public IHttpActionResult Post(MigrateAdminUserRequest request)
        {
            return AsAdmin(request, session =>
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

        public class MigrateAdminUserRequest : MigrationRequest
        {
            public MigrateAdminUserRequest(
                string requestUsername,
                string requestPassword)
                : base(requestUsername, requestPassword)
            {
            }
        }
    }
}