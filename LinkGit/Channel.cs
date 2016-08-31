using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace LinkGit
{
    [Table("Channels")]
    public class Channel
    {
        [Unique, PrimaryKey]
        public ulong ID { get; set; }
        public string Name { get; set; }
    }
}
