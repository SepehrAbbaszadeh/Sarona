using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.ViewModels
{
    public class DistrictViewModel
    {
        public Area SelectedDistrict { get; set; }
        public Abbreviation NewAbbreviation { get; set; }
        public IEnumerable<Abbreviation> Exchanges { get; set; }
    }
}
