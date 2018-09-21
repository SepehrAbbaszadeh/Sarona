using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public enum MiscType
    {
        Manufacturer=0,
        CoreType=1,
        CoreModel=2,
        AccessType=3,
        AccessModel=4,
        RemoteType=5,
        RemoteModel=6,
        Owner=7,
        Providence = 8,
        ChargingCase = 9,
        NumberType = 10,
        RoutingType = 11
    }
    public class Misc
    {
        public int Id { get; set; }
        public MiscType Type { get; set; }
        public string Name { get; set; }
    }
}
