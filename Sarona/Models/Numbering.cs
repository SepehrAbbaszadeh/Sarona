using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sarona.Models
{
    public enum NumberingStatus
    {
        Free,
        Reserved,
        Used
    }
    public abstract class NumberingPrefix
    {
        public long Id { get; set; }
        [Required]
        [RegularExpression("^[0-9*#]*$")]
        public string Prefix { get; set; }
        [Required]
        [Range(3, 30)]
        public byte Min { get; set; }
        [Required]
        [Range(3, 30)]
        public byte Max { get; set; }
        [Required]
        [Display(Name = "Charging")]
        public string ChargingCase { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Username { get; set; }
        public string Remark { get; set; }
    }

    public class NumberingPool : NumberingPrefix
    {
        [Required]
        public string Owner { get; set; }
        [Required]
        [Display(Name = "Type")]
        public string NumberType { get; set; }
        [Required]
        [Display(Name = "Routing Type")]
        public string RoutingType { get; set; }
        [Required]
        [Display(Name = "Expire Date")]
        public DateTime ExpireDate { get; set; }
        public NumberingStatus Status { get; set; }
        [Required]
        [Display(Name = "Float")]
        public bool IsFloat { get; set; }
        public DGSB Dgsb { get; set; }
        public IEnumerable<NumberingPoolNetworkElement> NumberingPoolNetworkElements { get; set; }
    }

    public class NumberingRouting : NumberingPrefix
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
