//-----------------------------------------------------------------------
// <copyright file="DiscordController.cs">
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
    /// The controller to interact with the Discord-net API.
    /// </summary>
    public class DiscordController
    {
        private DiscordClient _client;
        private SQLiteController _dbcontroler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscordController" /> class.
        /// </summary>
        /// <param name="logging">Set to false to disable logging. True by default</param>
        /// <param name="linking">Set to false to disable linking to Github. True by default</param>
        public DiscordController(bool logging = true, bool linking = true)
        {
            this._dbcontroler = new SQLiteController();
            this._dbcontroler.OpenDB(@"./Chatlog");
            this._client = new DiscordClient();
            this.Logging = logging;
            this.Linking = linking;

            this._client.MessageReceived += this._client_MessageReceived;

            this._client.MessageDeleted += this._client_MessageDeleted;

            this._client.MessageUpdated += this._client_MessageUpdated;

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

        /// <summary>
        /// Gets or sets a value indicating whether the controller should log messages or not
        /// </summary>
        public bool Logging { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the controller should link to Github pages or not
        /// </summary>
        public bool Linking { get; set; }

        private void _client_MessageUpdated(object sender, MessageUpdatedEventArgs e)
        {
            string atachments = string.Empty;
            foreach (Discord.Message.Attachment a in e.After.Attachments)
            {
                atachments += " |attachment: " + a.Url;
            }

            this._dbcontroler.EditMessage(e.Before.Id, e.After.Text + atachments);
        }

        private void _client_MessageDeleted(object sender, MessageEventArgs e)
        {
            this._dbcontroler.RemoveMessage(e.Message.Id);
        }

        private async void _client_MessageReceived(object sender, MessageEventArgs e)
        {                
            // Checks the message to see if there is a #[number] in the message
            Match m = Regex.Match(e.Message.Text, "#(\\d+)");
            if (!e.Message.IsAuthor && m.Success && this.Logging)
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

            string atachments = string.Empty;
            foreach (Discord.Message.Attachment a in e.Message.Attachments)
            {
                atachments += " |attachment: " + a.Url;
            }

            this._dbcontroler.AddMessage(e.Message.Id, e.Message.User.ToString(), e.Message.Text + atachments, e.Message.Timestamp, e.Channel.Id, e.Channel.Name);
        }
    }
}
