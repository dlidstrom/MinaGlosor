using System;
using System.Configuration;
using MinaGlosor.Tool.Migrations;

namespace MinaGlosor.Tool
{
    internal class Program
    {
        public const bool DryRun = false;

        private static void Main()
        {
            try
            {
                Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void Run()
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine();
            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            var serverUrl = ConfigurationManager.AppSettings["ServerUrl"];
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["MinaGlosorSqlServer"];

            var userMigration = new UserMigration(connectionStringSettings, username, password, serverUrl, DryRun);
            var wordListMigration = new WordListMigration(connectionStringSettings, username, password, serverUrl, DryRun);
            var wordMigration = new WordMigration(connectionStringSettings, username, password, serverUrl, DryRun);
            foreach (var migration in new IMigration[] { userMigration, wordListMigration, wordMigration })
            {
                migration.Migrate();
            }
        }
    }
}