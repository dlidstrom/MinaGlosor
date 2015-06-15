using System.Collections.Generic;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class RegisterAdminUsersCommand : ICommand<RegisterAdminUsersCommand.Result>
    {
        private readonly string[] adminUserIds;

        public RegisterAdminUsersCommand(string[] adminUserIds)
        {
            this.adminUserIds = adminUserIds;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return currentUser.IsAdmin;
        }

        public Result Execute(IDocumentSession session)
        {
            var websiteConfig = session.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            if (websiteConfig == null)
            {
                websiteConfig = new WebsiteConfig();
                session.Store(websiteConfig);
            }

            var migratedUsers = new List<string>();
            foreach (var adminUserId in adminUserIds)
            {
                if (websiteConfig.IsAdminUser(adminUserId) == false)
                {
                    websiteConfig.AddAdminUser(adminUserId);
                    migratedUsers.Add(adminUserId);
                }
            }

            return new Result(migratedUsers.ToArray());
        }

        public class Result
        {
            public Result(string[] migratedUsers)
            {
                MigratedUsers = migratedUsers;
            }

            public string[] MigratedUsers { get; private set; }
        }
    }
}