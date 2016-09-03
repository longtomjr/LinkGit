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
    using Mono.Options;

    /// <summary>
    /// The program
    /// </summary>
    public class Program
    {
        private DiscordController _discordControler;
        private SQLiteController _sqliteControler;

        /// <summary>
        /// Calls the Start function when running the program
        /// </summary>
        /// <param name="args">The args given at startup</param>
        public static void Main(string[] args) => new Program().Start(args);

        /// <summary>
        /// The function that runs at the start of the program
        /// </summary>
        /// <param name="args">arguments passed</param>
        public void Start(string[] args)
        {
            ulong startID = 0;
            ulong endID = 0;
            ulong channelID = 0;

            OptionSet options = new OptionSet
            {
                {"b|bot", "starts running the bot", b => {
                    this._discordControler = new DiscordController();
                } },

                {"s|startid=", "sets the startID for getting a list of messages", s => startID = ulong.Parse(s) },
                {"e|endid=", "sets the endID for getting a list of messages", s => endID = ulong.Parse(s) },
                {"c|channelid=", "sets the channelID for getting a list of messages", s => channelID = ulong.Parse(s) },

                {"m|messages", "gets the messages for the specified channel between two MessageIDs", m =>
                {
                    _sqliteControler = new SQLiteController();
                    _sqliteControler.OpenDB(@"./Chatlog");
                    _sqliteControler.PrintMessages(startID, endID, channelID);
                } },

            };
            options.Parse(args);

            Console.ReadLine();

        }
    }
}
