using Sarona.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.ViewModels
{
    public class RedFolderViewModel
    {
        public IEnumerable<PRA_SIP_Sarona> Records { get; set; }
        public string Abb { get; set; }
        public string Code { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public QueryMode Mode { get; set; }
    }
}
