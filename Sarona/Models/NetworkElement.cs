
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sarona.Models
{

    public enum NeType
    {
        Core,
        Access,
        Remote
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
        [Required]
        public string Owner { get; set; }
        public long? ParentId { get; set; }
        public NetworkElement Parent { get; set; }
        public string Remark { get; set; }
        public IEnumerable<Link> LinksOnEnd1 { get; set; }
        public IEnumerable<Link> LinksOnEnd2 { get; set; }
        public IEnumerable<NumberingPoolNetworkElement> NumberingPoolNetworkElements { get; set; }
        public IEnumerable<NetworkElement> NetworkElements { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Username { get; set; }
        public DateTime CreatedOn { get; set; }

        public string GetNumberings()
        {
            var numbers = new List<string>();
            foreach (var junction in NumberingPoolNetworkElements)
            {
                numbers.Add(junction.Numbering.Prefix);
            }
            return string.Join(',', numbers);
        }
    }
}
