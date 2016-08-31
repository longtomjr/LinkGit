using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Microsoft.AspNet.WebHooks;
using Discord.Logging;
using Microsoft.Extensions.Logging;

namespace LinkGit
{
    class Program
    {
        static void Main(string[] args) => new Program().Start();
        private DiscordClient _client;

        public void Start()
        {
            _client = new DiscordClient();
            _client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");

            Logger _logger = new Logger(_client);

            _client.MessageReceived += async (s, e) =>
            {

                _logger.Log(e.User.ToString(), e.Message.Id, e.Message.Timestamp.ToString(), e.Message.Text);

                if (!e.Message.IsAuthor)
                {
                    Match m = Regex.Match(e.Message.Text, "#(\\d+)");
                    if (m.Success)
                    {
                        string page = "https://github.com/TeamPorcupine/ProjectPorcupine/issues/" + m.ToString().Substring(1);
                        using (HttpClient pageChecker = new HttpClient())
                        {
                            var response = await pageChecker.GetAsync(page);

                            if (response.IsSuccessStatusCode)
                            {
                                await e.Channel.SendMessage(page);
                            }
                        }
                    }
                }
            };
            string token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (token != null)
            {
                _client.ExecuteAndWait(async () => {
                    await _client.Connect(token);
                });
            }
            else
            {
                _client.ExecuteAndWait(async () => {
                    await _client.Connect(System.IO.File.ReadAllText(@"./token"));
                });
            }
        }
    }
}
