using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.ViewModels
{
    public class NumberingPoolViewModel
    {
        public NumberingPool NewPrefix { get; set; }
        public NumberingPool EditPrefix { get; set; }
        public IEnumerable<Misc> Miscs { get; set; }
        public IEnumerable<NumberingPool> Prefixes { get; set; }
        public string Prefix { get; set; }
    }
}
