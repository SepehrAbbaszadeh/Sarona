using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public class CreateFreeCode
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string Type { get; set; }
        public int MyProperty { get; set; }
    }
}
