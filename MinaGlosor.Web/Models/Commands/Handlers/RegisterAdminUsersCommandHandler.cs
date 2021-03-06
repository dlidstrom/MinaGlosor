using System.Collections.Generic;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class RegisterAdminUsersCommandHandler : CommandHandlerBase<RegisterAdminUsersCommand, RegisterAdminUsersCommand.Result>
    {
        public override RegisterAdminUsersCommand.Result Handle(RegisterAdminUsersCommand command)
        {
            var websiteConfig = Session.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            if (websiteConfig == null)
            {
                websiteConfig = new WebsiteConfig();
                Session.Store(websiteConfig);
            }

            var migratedUsers = new List<string>();
            foreach (var adminUserId in command.AdminUserIds)
            {
                if (websiteConfig.IsAdminUser(adminUserId) == false)
                {
                    websiteConfig.AddAdminUser(adminUserId);
                    migratedUsers.Add(adminUserId);
                }
            }

            return new RegisterAdminUsersCommand.Result(migratedUsers.ToArray());
        }

        public override bool CanExecute(RegisterAdminUsersCommand command, User currentUser)
        {
            return currentUser.IsAdmin;
        }
    }
}