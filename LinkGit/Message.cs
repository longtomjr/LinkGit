using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace LinkGit
{
    [Table("Messages")]
    public class Message
    {
        [Unique, PrimaryKey]
        public ulong ID { get; set; }

        public string user { get; set; }

        public string text { get; set; }

        public DateTime timestamp { get; set; }

        public ulong channelID { get; set; }
    }
}
