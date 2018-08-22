using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.ViewModels
{
    public class RemotesViewModel
    {
        public NetworkElement NE { get; set; }
        public IEnumerable<NetworkElement> Remotes { get; set; }
    }
}
