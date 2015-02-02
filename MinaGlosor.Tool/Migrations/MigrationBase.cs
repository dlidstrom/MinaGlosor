using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;

namespace MinaGlosor.Tool.Migrations
{
    public abstract class MigrationBase : IMigration
    {
        private readonly ConnectionStringSettings connectionStringSettings;
        private readonly string username;
        private readonly string password;
        private readonly string serverUrl;
        private readonly bool dryRun;

        protected MigrationBase(ConnectionStringSettings connectionStringSettings, string username, string password, string serverUrl, bool dryRun)
        {
            this.connectionStringSettings = connectionStringSettings;
            this.username = username;
            this.password = password;
            this.serverUrl = serverUrl;
            this.dryRun = dryRun;
        }

        public void Migrate()
        {
            using (var connection = new SqlConnection(connectionStringSettings.ConnectionString))
            {
                connection.Open();

                var cmdText = GetItemSql();
                var updates = new List<string>();
                using (var command = new SqlCommand(cmdText, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (true)
                    {
                        var hasMore = reader.Read();
                        if (!hasMore)
                        {
                            Console.WriteLine("No more {0} to process", GetItemType());
                            break;
                        }

                        var tuple = ConvertToItem(username, password, reader);
                        Console.WriteLine(tuple.Item2);

                        if (dryRun)
                            continue;

                        using (var client = new HttpClient())
                        {
                            Console.WriteLine("Sending {0}", GetItemType());
                            var requestUri = string.Format("{0}{1}", serverUrl, GetUri());
                            var result = client.PostAsJsonAsync(requestUri, tuple.Item2).Result;
                            Console.WriteLine(result);
                            if (result.Content != null)
                            {
                                var content = result.Content.ReadAsStringAsync().Result;
                                Console.WriteLine(content);
                            }

                            if (result.StatusCode == HttpStatusCode.Created)
                            {
                                Console.WriteLine("{0} created successfully", GetItemType());
                                var updateSql = GetUpdateSql(tuple.Item1);
                                updates.Add(updateSql);
                            }
                            else if (result.StatusCode == HttpStatusCode.OK)
                            {
                                Console.WriteLine("{0} already exists", GetItemType());
                                var updateSql = GetUpdateSql(tuple.Item1);
                                updates.Add(updateSql);
                            }
                        }
                    }
                }

                foreach (var update in updates)
                {
                    Console.WriteLine("Executing '{0}'", update);
                    using (var updateCommand = new SqlCommand(update, connection))
                    {
                        updateCommand.ExecuteNonQuery();
                        var rowsAffected = updateCommand.ExecuteNonQuery();
                        Console.WriteLine("Updated {0} rows", rowsAffected);
                    }
                }
            }
        }

        protected abstract string GetItemType();

        protected abstract string GetItemSql();

        protected abstract Tuple<int, object> ConvertToItem(string username, string password, SqlDataReader reader);

        protected abstract string GetUri();

        protected abstract string GetUpdateSql(int id);
    }
}