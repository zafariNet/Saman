using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddSpecialNumberRequestOld
    {
        public int FromNumber { get; set; }
        public int ToNumber { get; set; }
        public string Note { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }

    public class AddSpecialNumberRequest
    {
        public int FromNumber { get; set; }
        public int ToNumber { get; set; }
        public string Note { get; set; }
    }
}
