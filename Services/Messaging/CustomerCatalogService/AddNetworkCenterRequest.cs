using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddNetworkCenterRequest
    {
        public Guid CenterID { get; set; }
        public Guid NetworkID { get; set; }
        public int Status { get; set; }
        public Guid CreateEmployeeID { get; set; }

        public bool CanSale { get; set; }
    }
}
