using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public class NumberingRange
    {
        public long From { get; set; }
        public long To { get; set; }
        public byte Min { get; set; }
        public byte Max { get; set; }
        public string NumberType { get; set; }
        public string ChargingCase { get; set; }
        public string MyProperty { get; set; }
    }
}
