using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public interface ICapacity
    {
        int InstalledCapacity { get; set; }
        int UsedCapacity { get; set; }
    }
    
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
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public long ExchangeId { get; set; }
        public Exchange Exchange { get; set; }
        [Required]
        public int InstalledCapacity { get; set; } 
        [Required]
        public int UsedCapacity { get; set; }
        public long? ParentId { get; set; }
        public NetworkElement Parent { get; set; }
        public string Remark { get; set; }
        public IEnumerable<Link> LinksOnEnd1 { get; set; }
        public IEnumerable<Link> LinksOnEnd2 { get; set; }
        public IEnumerable<NumberingPoolNetworkElement> NumberingPoolNetworkElements { get; set; }
        public long? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<NetworkElement> NetworkElements { get; set; }
    }
}
