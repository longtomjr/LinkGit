//-----------------------------------------------------------------------
// <copyright file="Message.cs">
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
    using SQLite;

    /// <summary>
    /// Class for a database object that represents a Discord Message
    /// </summary>
    [Table("Messages")]
    public class Message
    {
        /// <summary>
        /// Gets or sets the ID of the message
        /// </summary>
        [Unique, PrimaryKey]
        public long ID { get; set; }

        /// <summary>
        /// Gets or sets the user that sent the message
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the time the message was sent
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the ID of the channel the message was sent from
        /// </summary>
        public long ChannelID { get; set; }
    }
}
