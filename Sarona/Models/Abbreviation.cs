using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public enum AbbType
    {
        Exchange,
        Customer
    }
    public class Abbreviation
    {
        public long Id { get; set; }
        public AbbType Type { get; set; }
        public string Name { get; set; }
        public string Abb { get; set; }
        public Area Area { get; set; }

    }
}
