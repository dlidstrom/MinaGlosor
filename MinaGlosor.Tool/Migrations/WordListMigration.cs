using System;
using System.Configuration;
using System.Data.SqlClient;
using MinaGlosor.Tool.Dto;

namespace MinaGlosor.Tool.Migrations
{
    public class WordListMigration : MigrationBase
    {
        public WordListMigration(ConnectionStringSettings connectionStringSettings, string username, string password, string serverUrl, bool dryRun)
            : base(connectionStringSettings, username, password, serverUrl, dryRun)
        {
        }

        protected override string GetItemType()
        {
            return "WordList";
        }

        protected override string GetItemSql()
        {
            const string CmdText = "select WordLists.Id, WordLists.Name, Users.Email"
                                   + " from WordLists"
                                   + " join Users on WordLists.OwnerId = Users.Id"
                                   + " where WordLists.Migrated = 0";
            return CmdText;
        }

        protected override Tuple<int, object> ConvertToItem(string username, string password, SqlDataReader reader)
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);
            var ownerEmail = reader.GetString(2);
            object wordList = new WordList
            {
                RequestUsername = username,
                RequestPassword = password,
                Name = name,
                OwnerEmail = ownerEmail
            };
            return Tuple.Create(id, wordList);
        }

        protected override string GetUri()
        {
            return "/api/migratewordlist";
        }

        protected override string GetUpdateSql(int id)
        {
            var updateSql = string.Format("update WordLists set Migrated = 1 where Id = {0}", id);
            return updateSql;
        }
    }
}