using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Querying
{
    public class Sort
    {
        public Sort()
        {

        }
        public Sort(string sortColumn)
        {
            SortColumn = sortColumn;
            Asc = true;
        }

        public Sort(string sortColumn, bool asc)
        {
            SortColumn = sortColumn;
            Asc = asc;
        }

        public string SortColumn { get; set; }

        public bool Asc { get; set; }
    }
    public class simplesort {
        public string property { get; set; }
        public string direction { get; set; }
    }

    [Serializable]
    public class UiSort
    {
        public string property { get; set; }
        public string direction { get; set; }
    }
}
