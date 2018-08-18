using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.Infrastructure
{
    public class LinkChannelAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Link link = (Link)validationContext.ObjectInstance;

            if (link.Type == LinkType.ISUP && link.Channels%31 !=0)
            {
                return new ValidationResult("Number of channels for ISUP links must be N*31");
            }

            return ValidationResult.Success;
        }
        
    }
}
