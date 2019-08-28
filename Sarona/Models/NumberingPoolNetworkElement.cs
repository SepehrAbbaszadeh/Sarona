using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{

    public class NumberingPoolNetworkElement
    {
        public long NumberingId { get; set; }
        public NumberingPool Numbering { get; set; }
        public long NetworkElementId { get; set; }
        public NetworkElement Element { get; set; }
    }
}
