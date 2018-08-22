using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sarona.Models
{
    public class Abbreviation
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Abbreviation")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only!")]
        public string Abb { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Username { get; set; }
    }

    public class Exchange : Abbreviation
    {
        public Area Area { get; set; }
        public IEnumerable<NetworkElement> NetworkElements { get; set; }
    }
    public class Customer : Abbreviation
    {
        public IEnumerable<NetworkElement> Owned { get; set; }
    }

}
