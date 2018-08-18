using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Infrastructure;

namespace Sarona.Models
{
    public enum LinkType
    {
        ISUP,
        SIP
    }
    public enum LinkDirection
    {
        Incoming,
        Outgoing,
        Bothway
    }
    public class Link
    {
        public long Id { get; set; }
        [Required]
        public LinkType Type { get; set; }
        [Required]
        [Range(0,int.MaxValue,ErrorMessage ="Channels must be greater than 0.")]
        [LinkChannel]
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
        public string Remark { get; set; }
        public long? OtherLinkId { get; set; }
        public Link OtherLink { get; set; }

    }
}
