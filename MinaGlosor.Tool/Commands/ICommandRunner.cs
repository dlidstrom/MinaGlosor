using System;
using System.Net.Http;
using MinaGlosor.Web.Models.AdminCommands;

namespace MinaGlosor.Tool.Commands
{
    public interface ICommandRunner
    {
        void Run(string username, string password, string[] args);
    }

    public abstract class CommandRunner : ICommandRunner
    {
        public ServerUrlProvider ServerUrlProvider { get; set; }

        public void Run(string username, string password, string[] args)
        {
            using (var client = new HttpClient(new LoggingHandler(new HttpClientHandler())))
            {
                var requestUri = string.Format("{0}api/migrateadminuser", ServerUrlProvider.ServerUrl);
                var vm = CreateCommand(username, password, args);

                if (client.PostAsJsonAsync(requestUri, vm).Wait(TimeSpan.FromSeconds(30)) == false)
                {
                    Console.WriteLine("Request timed out");
                }
            }
        }

        protected abstract IAdminCommand CreateCommand(string username, string password, string[] args);
    }
}