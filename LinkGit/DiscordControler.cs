//-----------------------------------------------------------------------
// <copyright file="DiscordControler.cs">
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
    using System.Threading.Tasks;
    using Discord;
    using System.Text.RegularExpressions;

    public class DiscordControler
    {
        private DiscordClient _client;
        public bool Logging;
        public bool Linking;
        private SQLiteController _dbcontroler;

        public DiscordControler(bool logging = true, bool linking = true)
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

        private void _client_MessageUpdated(object sender, MessageUpdatedEventArgs e)
        {
            _dbcontroler.EditMessage(e.Before.Id, e.After.Text);
        }

        private void _client_MessageDeleted(object sender, MessageEventArgs e)
        {
            _dbcontroler.RemoveMessage(e.Message.Id);
        }

        async private void _client_MessageReceived(object sender, MessageEventArgs e)
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
            string atachments = "";
            foreach (Discord.Message.Attachment a in e.Message.Attachments)
            {
                atachments += " |attachment: " + a.Url;
            }
            this._dbcontroler.AddMessage(e.Message.Id, e.Message.User.ToString(), e.Message.Text + atachments, e.Message.Timestamp, e.Channel.Id, e.Channel.Name);
        }
    }
}
