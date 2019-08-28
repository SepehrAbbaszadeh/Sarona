using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public class FullPageRecord
    {
        public string Prefix { get; set; }
        public string Description { get; set; }
        public Color Color { get; set; }
        public int Row { get { return Convert.ToInt32(Prefix.Substring(0, 2)); } }

        public Dictionary<string,int> Types { get; set; }
        public Dictionary<string,int> LinkTypes { get; set; }
        public int Capacity { get; set; }
    }
}
