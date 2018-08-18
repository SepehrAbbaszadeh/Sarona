using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.ViewModels
{
    public class LinksViewModel
    {
        public NetworkElement NE { get; set; }
        public IEnumerable<Link> Links => NE.LinksOnEnd1;
        public IEnumerable<Misc> Misc { get; set; }
        public Link NewLink { get; set; }
    }
}
