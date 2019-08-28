using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sarona.Models
{
    public class Abbreviation
    {
        public string Abb { get; set; }
        public IEnumerable<Exchange> Exchange { get; set; }
        public IEnumerable<NumberingPool> NumbeingPools { get; set; }
    }

    
    //public class Customer : Abbreviation
    //{
    //    public IEnumerable<NetworkElement> Owned { get; set; }
    //}

}
