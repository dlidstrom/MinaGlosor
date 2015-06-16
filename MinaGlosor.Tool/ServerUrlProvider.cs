using System;

namespace MinaGlosor.Tool
{
    public class ServerUrlProvider
    {
        public ServerUrlProvider(string serverUrl)
        {
            if (serverUrl == null) throw new ArgumentNullException("serverUrl");
            ServerUrl = serverUrl;
        }

        public string ServerUrl { get; private set; }
    }
}