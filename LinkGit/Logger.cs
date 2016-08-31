using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Microsoft.Extensions.Logging;

namespace LinkGit
{
    class Logger
    {
        private DiscordClient _client;


        public Logger(DiscordClient client)
        {
            this._client = client;

        }

        public void Log(string user, ulong id, string time, string message)
        {
            string logstring = id.ToString() + "," + user + "," + message + "," + time;
            Console.WriteLine(logstring);
            File.WriteAllText(@"./log.log", logstring);
        }
    }
}
