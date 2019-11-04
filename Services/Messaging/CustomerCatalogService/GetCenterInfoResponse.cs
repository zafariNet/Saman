using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
using Model.Customers;

namespace Services.Messaging.CustomerCatalogService
{
    public class GetCenterInfoResponse
    {
        public Guid CenterID { get; set; }

        public string Status { get; set; }

        public Center Center { get; set; }

        public bool hasCenter { get; set; }

        public string CenterName { get; set; }
    }
}
