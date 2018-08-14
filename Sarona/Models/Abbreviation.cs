using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public enum AbbType
    {
        Exchange,
        Customer
    }
    public class Abbreviation
    {
        public long Id { get; set; }
        [Required]
        public AbbType Type { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Abbreviation")]
        [Remote("AbbValidation","Network")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only!")]
        public string Abb { get; set; }
        public Area? Area { get; set; }
        public DateTime CreatedOn { get; set; }
        public IEnumerable<NetworkElement> NetworkElements { get; set; }
    }
}
