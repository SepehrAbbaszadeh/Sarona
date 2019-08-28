using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public class LinkHistory
    {
        public long Id { get; set; }
        public LinkType Type { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Username { get; set; }
        public string Remark { get; set; }
        public int Channels { get; set; }
        public long LinkId { get; set; }
        public LinkDirection Direction { get; set; }
    }
}
