using System;
using System.Configuration;
using System.Net.Http;

namespace MinaGlosor.Tool.Commands
{
    public class MigrateAdminUserCommand : ICommand
    {
        private readonly string serverUrl;

        public MigrateAdminUserCommand()
        {
            serverUrl = ConfigurationManager.AppSettings["ServerUrl"];
        }

        public void Run(string username, string password, string[] args)
        {
            using (var client = new HttpClient(new LoggingHandler(new HttpClientHandler())))
            {
                var requestUri = string.Format("{0}api/migrateadminuser", serverUrl);
                var vm = new
                {
                    RequestUsername = username,
                    RequestPassword = password
                };

                if (client.PostAsJsonAsync(requestUri, vm).Wait(TimeSpan.FromSeconds(30)) == false)
                {
                    Console.WriteLine("Timed out");
                }
            }
        }
    }
}