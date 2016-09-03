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
        private DiscordControler _dcControler;

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
            this._dcControler = new DiscordControler();

        }
    }
}
