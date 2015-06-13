using System;
using System.Collections.Generic;

namespace MinaGlosor.Web.Models
{
    public class WebsiteConfig
    {
        public const string GlobalId = "WebsiteConfig";

        public WebsiteConfig()
        {
            Id = GlobalId;
            IndexCreatedVersion = string.Empty;
            AdminUsers = new HashSet<string>();
        }

        public string Id { get; private set; }

        public string IndexCreatedVersion { get; private set; }

        public HashSet<string> AdminUsers { get; private set; }

        public void SetIndexCreatedVersion(string version)
        {
            if (version == null) throw new ArgumentNullException("version");
            IndexCreatedVersion = version;
        }

        public bool IsAdminUser(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            var isAdminUser = AdminUsers.Contains(userId);
            return isAdminUser;
        }

        public void AddAdminUser(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            AdminUsers.Add(userId);
        }
    }
}