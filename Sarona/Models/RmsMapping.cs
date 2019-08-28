using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public class RmsMapping
    {
        public long Id { get; set; }
        public string Markaz { get; set; }
        public string RouteName { get; set; }
        public long MarkazId { get; set; }
        public long OperatorId { get; set; }

    }
}
