using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sarona.Models
{


    public class NumberingPool : IValidatableObject
    {
        public long Id { get; set; }
        [Required]
        [RegularExpression("^[0-9*#]*$")]
        public string Prefix { get; set; }
        [Required]
        [Range(1, 30)]
        public byte Min { get; set; }
        [Required]
        [Range(1, 30)]
        public byte Max { get; set; }
        public LinkDirection? Direction { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Username { get; set; }
        public string Remark { get; set; }
        [Required]
        [Display(Name = "Charging")]
        public string ChargingCase { get; set; }
        [Required]
        public string Owner { get; set; }
        public NumberingStatus Status { get; set; }
        public IEnumerable<NumberingPoolNetworkElement> NumberingPoolNetworkElements { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Min > Max)
            {
                yield return new ValidationResult("\"Max\" must be greater or equal to \"Min\".");
            }
            if (SecondaryMin > SecondaryMax)
            {
                yield return new ValidationResult("\"Secondary Max\" must be greater or equal to \"Secondary Min\".");
            }
            if (!string.IsNullOrEmpty(Abb) && string.IsNullOrEmpty(SubscriberName))
            {
                yield return new ValidationResult("Please insert subscriber name.");
            }
        }
        [Required]
        [Display(Name = "Float")]
        public bool IsFloat { get; set; }
        public string[] GetNetworkElementNames()
        {
            if (NumberingPoolNetworkElements is null || NumberingPoolNetworkElements.Count() == 0)
            {
                return null;
            }

            string[] result = new string[NumberingPoolNetworkElements.Count()];
            int i = 0;
            foreach (var ne in NumberingPoolNetworkElements)
            {
                switch (ne.Element.NetworkType)
                {
                    case NeType.Core:
                        result[i++] = $"{ne.Element.Exchange.Abb} ({ne.Element.Name}-{ne.Element.Model})";
                        break;
                    case NeType.Access:
                    case NeType.Remote:
                        result[i++] = $"{ne.Element.Exchange.Abb} ({ne.Element.Model}-{ne.Element.Parent.Name})";
                        break;
                    
                    default:
                        break;
                }
                
            }
            return result;
        }
        public string GetNetworkElementNamesHtml()
        {
            if (GetNetworkElementNames() is null)
            {
                return "No NE";
            }
            return string.Join(",", GetNetworkElementNames());
        }
        public Abbreviation Abbreviation { get; set; }
        [Required]
        [Display(Name = "Min (Sheet)")]
        public byte SecondaryMin { get; set; }
        [Display(Name = "Max (Sheet)")]
        [Required]
        public byte SecondaryMax { get; set; }
        public string Abb { get; set; }
        public string SubscriberName { get; set; }
        public string NormalizedSubscriberName { get; set; }
        [Required]
        [Display(Name = "Keshvari")]
        public bool IsKeshvari { get; set; } = false;
        [Required]
        [Display(Name = "Expire Date")]
        public DateTime ExpireDate { get; set; }
        [Display(Name = "Link Type")]
        public LinkType? Link { get; set; }
        public Area? Area { get; set; }
        [Display(Name = "Area (Sheet)")]
        public Area? SecondaryArea { get; set; }
        [Required]
        public GSB Rond { get; set; }
        [Required]
        [Display(Name = "Number Type")]
        public string NumberType { get; set; }
        public GSB GetRondType()
        {
            return RondType(Prefix);
        }
        public static GSB RondType(string number)
        {
            // 4 Digit number
            if (number.Length == 4)
            {
                char a = number[0];
                char b = number[1];
                char c = number[2];
                char d = number[3];

                //GOLD
                if (a == c && b == d)
                {
                    return GSB.Gold;
                }

                if (c == '0' && d == '0')
                {
                    return GSB.Gold;
                }

                if (b == c && c == d)
                {
                    return GSB.Gold;
                }

                if (a == d && d == b + 1)
                {
                    return GSB.Gold;
                }

                //SILVER
                if (c == d)
                {
                    return GSB.Silver;
                }

                //BRONZE
                if (b == d)
                {
                    return GSB.Bronze;
                }
            }
            // 5 Digit number
            else if(number.Length == 5)
            {
                char a = number[0];
                char b = number[1];
                char c = number[2];
                char d = number[3];
                char e = number[4];

                //GOLD
                if (c == d && d == e)
                {
                    return GSB.Gold;
                }

                if (c == b + 1 && d == c + 1 && e == d + 1)
                {
                    return GSB.Gold;
                }

                if (e == d - 1 && d == c - 1 && c == b - 1)
                {
                    return GSB.Gold;
                }

                //SILVER
                if (a == d && b == e)
                {
                    return GSB.Silver;
                }

                if (d == '0' && e == '0')
                {
                    return GSB.Silver;
                }

                //BRONZE
                if (c == d - 1 && d == e - 1)
                {
                    return GSB.Bronze;
                }

                if (d == c - 1 && e == d - 1)
                {
                    return GSB.Bronze;
                }

                if (c == d && d == '0')
                {
                    return GSB.Bronze;
                }

            }
            return GSB.Ordinary;
        }
    }


}
