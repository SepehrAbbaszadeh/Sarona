using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.ViewModels
{
    public enum SearchRecordType
    {
        Exchange,
        Number,
        NE
    }
    public class SearchRecord
    {
        public SearchRecordType Type { get; set; }
        public string Text { get; set; }
        public string HtmlLink { get; set; }
    }
    public class SearchViewModel
    {
        public IEnumerable<SearchRecord> Records { get; set; }
        public string Query { get; set; }

    }
}
