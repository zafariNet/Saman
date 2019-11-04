using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Querying
{
    public class data1
    {
        public string type { get; set; }
        public string[] value { get; set; }
        public string comparsion { get; set; }
    }

    public class FilterModel
    {
        public string type { get; set; }
        public string comparsion { get; set; }
        public string[] value { get; set; }
        public string field { get; set; }
       
    }
}
