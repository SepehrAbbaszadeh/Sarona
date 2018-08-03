using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public enum LinkType
    {
        ISUP,
        SIP
    }
    public enum LinkDirection
    {
        Incoming,
        Outgoing,
        Bothway
    }
    public class Link
    {
        public long Id { get; set; }
        public LinkType Type { get; set; }
        public int ChannelNo { get; set; }
        public LinkDirection Direction { get; set; }
        public long End1Id { get; set; }
        public NetworkElement End1 { get; set; }
        public long End2Id { get; set; }
        public NetworkElement End2 { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Remark { get; set; }
        public long OtherLinkId { get; set; }
        public Link OtherLink { get; set; }

    }
}
