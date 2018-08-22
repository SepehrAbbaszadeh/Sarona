using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.ViewModels
{
    public class CapacitySpecs
    {
        public NeType Type { get; set; }
        public long SumInstalled { get; set; }
        public long SumUsed { get; set; }
        public int Count { get; set; }
    }

    public class LinksSpecsByNeType
    {
        public NeType Type { get; set; }
        public long Count { get; set; }
        public long SumChannels { get; set; }
    }

    public class LinksSpecsByLinkType
    {
        public LinkType Type { get; set; }
        public long Count { get; set; }
        public long SumChannels { get; set; }
    }

    public class CoreViewModel
    {
        public NetworkElement NE { get; set; }

        public CapacitySpecs Remotes { get; set; }
        public CapacitySpecs Accesses { get; set; }

        public LinksSpecsByLinkType SipLinks { get; set; }
        public LinksSpecsByLinkType IsupLinks { get; set; }
        public LinksSpecsByNeType CoreLinks { get; set; }
        public LinksSpecsByNeType PbxLinks { get; set; }
        public LinksSpecsByNeType IpPbxLinks { get; set; }
        public IEnumerable<Misc> Miscs { get; set; }
    }
}
