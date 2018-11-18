using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetasiteApplication.Data
{
    public class GridItemViewModel
    {
        public string Msisdn { get; set; }
        public int? Duration { get; set; }
        public DateTime? Date { get; set; }
        public int? Count { get; set; }
    }
}
