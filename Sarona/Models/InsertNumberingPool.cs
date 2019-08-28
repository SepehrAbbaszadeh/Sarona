using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.Models
{
    public class InsertNumberingPool:  IValidatableObject
    {
        [Required]
        [Range(0,int.MaxValue)]
        public int From { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int To { get; set; }
        [Required]
        [Range(1, 30)]
        public byte Min { get; set; }
        [Required]
        [Range(1, 30)]
        public byte Max { get; set; }
        [Required]
        public Area Area { get; set; }
        [Required]
        public string NumberType { get; set; }
        [Display(Name = "Link Type")]
        public LinkType? Link { get; set; }
        [Required]
        [Display(Name = "Charging")]
        public string ChargingCase { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Min > Max)
            {
                yield return new ValidationResult("\"Max\" must be greater or equal to \"Min\".");
            }

            if(From>To)
            {
                yield return new ValidationResult("\"To\" must be greater or equal to \"From\".");
            }
        }
        [Display(Name ="Reserve")]
        public bool IsReserved { get; set; }
        [Display(Name ="Warning!!! Fill empty prefixes and get data from crm.")]
        public bool  NotDefined{ get; set; }

    }
}
