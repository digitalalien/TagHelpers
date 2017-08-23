using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TagHelpersTest.Models
{
    public class SearchViewModel
    {
        public string SearchTerm { get; set; }
        public bool IncludeArchived { get; set; }
        public DateTime FromDate { get; set; }
    }
}
