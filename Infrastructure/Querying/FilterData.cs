using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Querying
{
    public class data 
    {
        public string type { get; set; }
        public string[] value { get; set; }
        public string comparison { get; set; }
    }
    public class FilterData
    {
        public string field { get; set; }
        public data data { get; set; }

    }

}
