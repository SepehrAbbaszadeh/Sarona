using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.ViewModels
{
    public class ProvidenceExchanges
    {
        public string Providence { get; set; }
        public IEnumerable<Exchange> Exchanges { get; set; }
    }
    public class DistrictViewModel
    {
        public Area SelectedDistrict { get; set; }
        public Exchange NewExchange { get; set; }
        public IEnumerable<ProvidenceExchanges> Exchanges { get; set; }
        public IEnumerable<Misc> Miscs { get; set; }
    }
}
