using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public class NetworkElement
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public long AbbreviationId { get; set; }
        public Abbreviation Abbreviation { get; set; }
        public string Remark { get; set; }
        public IEnumerable<Link> LinksOnEnd1 { get; set; }
        public IEnumerable<Link> LinksOnEnd2 { get; set; }

    }
}
