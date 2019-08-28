using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public class PRA_SIP_Sarona
    {
        public string SwitchAbb { get; set; }
        public string SwitchCode { get; set; }
        public string SubscriberAbb { get; set; }
        public string SubscriberCode { get; set; }
        public int Channels { get; set; }
        public string LinkType { get; set; }
        public string Remark { get; set; }
    }
}
