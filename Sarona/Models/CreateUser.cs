using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public class CreateUser
    {
        [Remote("IsUsernameUnique","Account")]
        [Required]
        [RegularExpression("[a-zA-Z0-9_.]{5,15}",ErrorMessage = "Use only letters, numbers, dot and underscore. Username must start with a letter. Minimum length:5 Maximum length:15")]
        public string Username { get; set; }
        [Required]
        [UIHint("password")]
        //[RegularExpression("{5,15}", ErrorMessage = "Use only letters, numbers and underscore.\nUsername must start with a letter.\nMinimum length:5 Maximum length:15")]
        public string Password { get; set; }


    }
}
