using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.ViewModels
{
    public class CoreViewModel
    {
        public NetworkElement NE { get; set; }
        public IEnumerable<NetworkElement> Remotes => NE.NetworkElements.Where(x => x.NetworkType == NeType.Remote);
        public IEnumerable<NetworkElement> Accesses => NE.NetworkElements.Where(x => x.NetworkType == NeType.Access);
        public IEnumerable<Link> Links => NE.LinksOnEnd1;
        public IEnumerable<NumberingRange> Numberings => NE.NumberingPoolNetworkElements.Select(x => x.Numbering);
        public IEnumerable<Misc> Miscs { get; set; }
    }
}
