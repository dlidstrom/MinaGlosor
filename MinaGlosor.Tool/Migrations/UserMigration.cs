using System;
using System.Configuration;
using System.Data.SqlClient;
using MinaGlosor.Tool.Dto;

namespace MinaGlosor.Tool.Migrations
{
    public class UserMigration : MigrationBase
    {
        public UserMigration(ConnectionStringSettings connectionStringSettings, string username, string password, string serverUrl, bool dryRun)
            : base(connectionStringSettings, username, password, serverUrl, dryRun)
        {
        }

        protected override string GetItemType()
        {
            return "User";
        }

        protected override string GetItemSql()
        {
            return "select Id, Email, HashedPassword, CreatedDate from Users where Migrated = 0 and Role = 1";
        }

        protected override Tuple<int, object> ConvertToItem(string username, string password, SqlDataReader reader)
        {
            var id = reader.GetInt32(0);
            var email = reader.GetString(1);
            var hashedPassword = reader.GetString(2);
            var createdDate = reader.GetDateTime(3);
            object user = new UserDto
            {
                RequestUsername = username,
                RequestPassword = password,
                Email = email,
                Username = email.Substring(0, email.IndexOf("@", StringComparison.Ordinal)),
                HashedPassword = hashedPassword,
                CreatedDate = createdDate
            };
            return Tuple.Create(id, user);
        }

        protected override string GetUri()
        {
            return "/api/migrateuser";
        }

        protected override string GetUpdateSql(int id)
        {
            return string.Format("update Users set Migrated = 1 where Id = {0}", id);
        }
    }
}