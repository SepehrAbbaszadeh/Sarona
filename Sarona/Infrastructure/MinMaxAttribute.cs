using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Sarona.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sarona.Infrastructure
{
    public class MinMaxAttribute : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var number = (NumberingPool)validationContext.ObjectInstance;
            if (number.Min > number.Max)
                return new ValidationResult("Min must be equal or less than Max.");
            return ValidationResult.Success;
        }


        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-minmax", "Check min and max.");
        }

        private static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value); return true;
        }


    }
}
