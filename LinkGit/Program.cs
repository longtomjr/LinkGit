using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace LinkGit
{
    class Program
    {
        static void Main(string[] args) => new Program().Start();
        private DiscordClient _client;

        public void Start()
        {
            _client = new DiscordClient();

            _client.MessageReceived += async (s, e) =>
            {
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

            _client.ExecuteAndWait(async () => {
                await _client.Connect("MjIwMjI0ODgyNDAxNzM4NzUz.Cqdd-g.vuFGt54A3nk3w_IZjx9QK0MTEtg");
            });
        }
    }
}
