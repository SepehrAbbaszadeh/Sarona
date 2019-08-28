using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.ViewModels
{
    public class NumberingSearchViewModel
    {
        public NpSearch Search { get; set; }
        public NpSearch ExcelSearch { get; set; }
        public PagingInfo PageInfo { get; set; }
        public IEnumerable<NumberingPool> Results { get; set; }
        public IEnumerable<Misc> Miscs { get; set; }
    }
}
