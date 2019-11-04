using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;

namespace Services.ViewModels.Customers
{
    public class SimpleCustomerView
    {
        public Guid CustomerID { get; set; }

        public string FirstName { get; set; }

        public string Lastname { get; set; }

        public string LevelTitle { get; set; }
    }
}
