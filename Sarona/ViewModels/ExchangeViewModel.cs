using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.ViewModels
{
    public class ExchangeViewModel
    {
        public Area SelectedDistrict { get; set; }
        public Exchange SelectedExchange { get; set; }
        public IEnumerable<Abbreviation> Exchanges { get; set; }
        public IEnumerable<NetworkElement> CoreElements => SelectedExchange.NetworkElements.Where(x => x.NetworkType == NeType.Core);
        public IEnumerable<NetworkElement> Remotes => SelectedExchange.NetworkElements.Where(x => x.NetworkType == NeType.Remote);
        public IEnumerable<NetworkElement> Accesses => SelectedExchange.NetworkElements.Where(x => x.NetworkType == NeType.Access);
        public IEnumerable<NetworkElement> Pbxs => SelectedExchange.NetworkElements.Where(x => x.NetworkType == NeType.PBX);
        public IEnumerable<NetworkElement> IpPbxs => SelectedExchange.NetworkElements.Where(x => x.NetworkType == NeType.IP_PBX);
        public NetworkElement NewNE { get; set; }
        public IEnumerable<Misc> Miscs { get; set; }

    }
}
