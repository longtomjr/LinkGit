//-----------------------------------------------------------------------
// <copyright file="Program.cs">
//    This file is part of LinkGit.
//
//    LinkGit is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    LinkGit is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with LinkGit.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//-----------------------------------------------------------------------
namespace LinkGit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Discord;

    /// <summary>
    /// The program
    /// </summary>
    public class Program
    {
        private DiscordClient _client;

        /// <summary>
        /// Calls the Start function when running the program
        /// </summary>
        /// <param name="args">The args given at startup</param>
        public static void Main(string[] args) => new Program().Start();

        /// <summary>
        /// The function that is run at the start of the program
        /// </summary>
        public void Start()
        {
            var dbcontroler = new SQLiteController();
            dbcontroler.OpenDB(@"./Chatlog");

            this._client = new DiscordClient();

            // Checks for event MessageReceived
            this._client.MessageReceived += async (s, e) =>
            {
                // Checks that the bot is not the author (prevents echo loop)
                if (!e.Message.IsAuthor)
                {
                    // Checks the message to see if there is a #[number] in the message
                    Match m = Regex.Match(e.Message.Text, "#(\\d+)");
                    if (m.Success)
                    {
                        // create a url for the issue page (removes the '#')
                        string page = "https://github.com/TeamPorcupine/ProjectPorcupine/issues/" + m.ToString().Substring(1);

                        // Checks if the page exists
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

                dbcontroler.AddMessage(e.Message.Id, e.Message.User.ToString(), e.Message.Text, e.Message.Timestamp, e.Channel.Id, e.Channel.Name);
            };

            dbcontroler.PrintMessages(221343758124449802, 221344145011113984);

            this._client.MessageDeleted += (s, e) =>
            {
                dbcontroler.RemoveMessage(e.Message.Id);
            };

            this._client.MessageUpdated += (s, e) =>
            {
                dbcontroler.EditMessage(e.Before.Id, e.After.Text);
            };
             string token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (token != null)
            {
                this._client.ExecuteAndWait(async () => 
                {
                    await _client.Connect(token);
                });
            }
            else
            {
                this._client.ExecuteAndWait(async () => 
                {
                    await _client.Connect(System.IO.File.ReadAllText(@"./token"));
                });
            }
        }
    }
}
