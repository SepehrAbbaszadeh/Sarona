using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public enum NewNumberingPoolType
    {
        City,
        Enterprise,
        Tel30
    }
    public class NewNumberingPool
    {
        public NewNumberingPoolType Type { get; set; }
        public int  Test { get; set; }
    }
}
