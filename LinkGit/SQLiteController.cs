//-----------------------------------------------------------------------
// <copyright file="SQLiteController.cs">
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
    using System.Text;
    using System.Threading.Tasks;
    using SQLite;

    /// <summary>
    /// Controller for SQLite database
    /// </summary>
    public class SQLiteController
    {
        private SQLiteConnection _db;
        
        /// <summary>
        /// Opens or creates a database
        /// </summary>
        /// <param name="path">The location of the database</param>
        public void OpenDB(string path)
        {
            this._db = new SQLiteConnection(path);
            this._db.CreateTable<Message>();
            this._db.CreateTable<Channel>();
        }

        /// <summary>
        /// Adds a message to the database
        /// </summary>
        /// <param name="id">The Id of the message</param>
        /// <param name="user">The name of the user that sent the message</param>
        /// <param name="text">The message</param>
        /// <param name="timestamp">The time and date the message was sent</param>
        /// <param name="channelID">The Id of the channel the message was sent from</param>
        /// <param name="channelName">The name of the channel the message was sent from</param>
        public void AddMessage(ulong id, string user, string text, DateTime timestamp, ulong channelID, string channelName)
        {
            var newMessage = new Message();
            newMessage.ID = (long)id;
            newMessage.User = user;
            newMessage.Text = text;
            newMessage.Timestamp = timestamp;
            newMessage.ChannelID = (long)channelID;

            Console.WriteLine("newMessage Added: " + id.ToString() + ", " + user + ", " + text + ", " + timestamp.ToString() + ", " + channelID.ToString() + ", " + channelName);

            this._db.Insert(newMessage);

            try
            {
                var channel = this._db.Get<Channel>((long)channelID);
            }
            catch
            {
                var newChannel = new Channel();
                newChannel.ID = (long)channelID;
                newChannel.Name = channelName;
                this._db.Insert(newChannel);
            }
        }
    }
}
