using System.Configuration;
using MinaGlosor.Tool.Migrations;

namespace MinaGlosor.Tool.Commands
{
    public class InitialMigrationCommand : ICommand
    {
        private const bool DryRun = true;

        public void Run(string username, string password, string[] args)
        {
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