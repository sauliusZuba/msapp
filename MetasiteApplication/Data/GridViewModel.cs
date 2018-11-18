using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetasiteApplication.Data
{
    public class GridViewModel
    {
        public GridViewModel()
        {
            Items = new List<GridItemViewModel>();
            ColumnVisibilityMap = new string[] { };
        }

        public List<GridItemViewModel> Items { get; set; }
        public string [] ColumnVisibilityMap { get; set; }
        public bool ShowCount { get; set; }
        public string Name { get; set; }
    }
}
