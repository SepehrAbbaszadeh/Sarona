using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public class Exchange
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Abbreviation")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only!")]
        public string Abb { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Username { get; set; }
        public string Providence { get; set; }
        public Area Area { get; set; }
        public IEnumerable<NetworkElement> NetworkElements { get; set; }
        public Abbreviation Abbreviation { get; set; }
        [Display(Name="Site")]
        public bool IsSite { get; set; }
    }

}
