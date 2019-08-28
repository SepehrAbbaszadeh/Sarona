using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.ViewModels
{
    public class ReserveCodes
    {
        public string SelectedPrefixes { get; set; }
        public byte Min { get; set; }
        public byte Max { get; set; }
        public Area? Area { get; set; }
        public Link Link { get; set; }
    }
    public class NumberingPoolViewModel
    {
        public NumberingPool NewPrefix { get; set; }
        public NumberingPool EditPrefix { get; set; }
        public IEnumerable<Misc> Miscs { get; set; }
        public IEnumerable<NumberingPool> Prefixes { get; set; }
        public string Prefix { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public InsertNumberingPool FreeCode { get; set; }
        [Display(Name ="Reserve")]
        public bool IsReservedPrefix { get; set; }
        public ReserveCodes Reserve { get; set; }
    }
}
