using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public enum NeType
    {
        Core,
        Access,
        Remote,
        PBX,
        IP_PBX
    }
    public class NetworkElement
    {
        public long Id { get; set; }

        public string Name { get; set; }
        [Required]
        public NeType NetworkType { get; set; }
        [Required]
        public string Type { get; set; }
        //Huawei-Alcatel
        public string Manufacturer { get; set; }
        //umg8900-softx3000-NEAX-S12
        public string Model { get; set; }
        [Required]
        public long AbbreviationId { get; set; }
        public Abbreviation Abbreviation { get; set; }
        public int InstalledCapacity { get; set; } = 0;
        public int UsedCapacity { get; set; } = 0;
        public long? ParentId { get; set; }
        public NetworkElement Parent { get; set; }
        public string Remark { get; set; }
        public IEnumerable<Link> LinksOnEnd1 { get; set; }
        public IEnumerable<Link> LinksOnEnd2 { get; set; }
        public IEnumerable<NumberingPoolNetworkElement> NumberingPoolNetworkElements { get; set; }
        //public long OwnerId { get; set; }
        //public Abbreviation Owner { get; set; }
    }
}
