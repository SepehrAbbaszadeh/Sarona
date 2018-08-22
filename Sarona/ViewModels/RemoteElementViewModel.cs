using Sarona.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.ViewModels
{
    public class RemoteElementViewModel
    {
        public NetworkElement NE { get; set; }
        public IEnumerable<Misc> Miscs { get; set; }
    }
}
