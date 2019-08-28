using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.ViewModels
{
    public class NeNumberingViewModel
    {
        public NetworkElement Nes { get; set; }
        public bool IncludeChildren { get; set; }
        public long? Id { get; set; }
        public PagingInfo Paging { get; set; }
    }
}
