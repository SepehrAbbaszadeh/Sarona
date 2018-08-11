using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public abstract class NumberingRange
    {
        public long Id { get; set; }
        public long From { get; set; }
        public long To { get; set; }
        public byte Min { get; set; }
        public byte Max { get; set; }
        public string NumberType { get; set; }
        public string ChargingCase { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserName { get; set; }
        public string Remark { get; set; }
        public DGSB Dgsb { get; set; }
        public string RoutingType { get; set; }
        public DateTime ExpireDate { get; set; }
    }

    public class NumberingPool : NumberingRange
    {
        public IEnumerable<NumberingPoolNetworkElement> NumberingPoolNetworkElements { get; set; }
    }

    public class NumberingRouting : NumberingRange
    {

    }

    public class NumberingPoolNetworkElement
    {
        public long NumberingPoolId { get; set; }
        public NumberingPool Numbering { get; set; }
        public long NetworkElementId { get; set; }
        public NetworkElement Element { get; set; }

    }

}
