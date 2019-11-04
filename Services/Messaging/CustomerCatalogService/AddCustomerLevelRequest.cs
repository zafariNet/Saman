using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddCustomerLevelRequest
    {
        public Guid NewLevelID { get; set; }
        public Guid CustomerID { get; set; }
        public string Note { get; set; }
        public Guid CreateEmployeeID { get; set; }
        public Guid OldLevelID { get; set; }
        public bool NewCustomer { get; set; }
    }
}
