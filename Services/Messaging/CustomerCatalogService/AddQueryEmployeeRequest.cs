using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddQueryEmployeeRequestOld
    {
        public Guid QueryID { get; set; }
        public Guid EmployeeID { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }

    public class AddQueryEmployeeRequest
    {
        //public Guid QueryID { get; set; }
        public Guid EmployeeID { get; set; }
    }
}
