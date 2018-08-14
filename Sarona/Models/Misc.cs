using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public enum MiscType
    {
        Manufacturer,
        CoreType,
        RemoteType,
        AccessType,
        Model
    }
    public class Misc
    {
        public int Id { get; set; }
        public MiscType Type { get; set; }
        public string Name { get; set; }
    }
}
