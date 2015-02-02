using System;
using System.Configuration;
using System.Data.SqlClient;
using MinaGlosor.Tool.Dto;

namespace MinaGlosor.Tool.Migrations
{
    public class WordMigration : MigrationBase
    {
        public WordMigration(ConnectionStringSettings connectionStringSettings, string username, string password, string serverUrl, bool dryRun)
            : base(connectionStringSettings, username, password, serverUrl, dryRun)
        {
        }

        protected override string GetItemType()
        {
            return "Word";
        }

        protected override string GetItemSql()
        {
            const string CmdText = "select Words.Id, Words.CreatedDate, Words.Text, Words.Definition, WordLists.Name, Users.Email"
                                   + " from Words"
                                   + " join WordLists on Words.WordListId = WordLists.Id"
                                   + " join Users on WordLists.OwnerId = Users.Id"
                                   + " where Words.Migrated = 0";
            return CmdText;
        }

        protected override Tuple<int, object> ConvertToItem(string username, string password, SqlDataReader reader)
        {
            var id = reader.GetInt32(0);
            var createdDate = reader.GetDateTime(1);
            var text = reader.GetString(2);
            var definition = reader.GetString(3);
            var wordListName = reader.GetString(4);
            var ownerEmail = reader.GetString(5);
            object word = new Word
            {
                RequestUsername = username,
                RequestPassword = password,
                CreatedDate = createdDate,
                Text = text,
                Definition = definition,
                WordListName = wordListName,
                OwnerEmail = ownerEmail
            };
            return Tuple.Create(id, word);
        }

        protected override string GetUri()
        {
            return "/api/migrateword";
        }

        protected override string GetUpdateSql(int id)
        {
            var updateSql = string.Format("update Words set Migrated = 1 where Id = {0}", id);
            return updateSql;
        }
    }
}