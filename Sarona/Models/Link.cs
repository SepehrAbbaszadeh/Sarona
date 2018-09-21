using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public enum LinkType
    {
        ISUP,
        SIP,
        PRA
    }
    public enum LinkDirection
    {
        Incoming,
        Outgoing,
        Bothway
    }

    public class LinkHistory
    {
        public long Id { get; set; }
        public LinkType Type { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Username { get; set; }
        public string Remark { get; set; }
        public int Channels { get; set; }
        public long LinkId { get; set; }
        public LinkDirection Direction { get; set; }

    }
    
    public class Link:IValidatableObject
    {
        public long Id { get; set; }
        [Required]
        public LinkType Type { get; set; }
        [Range(0,int.MaxValue,ErrorMessage ="Channels must be greater than 0.")]
        public int Channels { get; set; }
        [Required]
        public LinkDirection Direction { get; set; }
        [Required]
        public long End1Id { get; set; }
        public NetworkElement End1 { get; set; }
        [Required]
        public long End2Id { get; set; }
        public NetworkElement End2 { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Username { get; set; }
        public string Remark { get; set; }
        public long? OtherLinkId { get; set; }
        public Link OtherLink { get; set; }

        public IEnumerable<LinkHistory> Histories { get; set; }

        public string GetStm1E1()
        {
            var stm1 = Math.Floor((double)Channels / 1953);
            var e1 = Math.Floor((Channels - stm1 * 1953) / 31);
            var channels = Channels - stm1 * 1953 - e1 * 31;
            return $"[{stm1},{e1},{channels}]";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Type == LinkType.ISUP && Channels % 31 != 0)
                yield return new ValidationResult("For ISUP links number of channels must be N*31.");
        }
    }
}
