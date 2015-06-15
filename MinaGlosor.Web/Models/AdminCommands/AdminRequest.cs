using System;
using Newtonsoft.Json;

namespace MinaGlosor.Web.Models.AdminCommands
{
    public class AdminRequest
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public AdminRequest(IAdminCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            CommandJson = JsonConvert.SerializeObject(command, Settings);
        }

        public string CommandJson { get; private set; }
    }
}