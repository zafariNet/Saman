using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;


namespace Services.Messaging.CustomerCatalogService
{
    public class GetSuctionModesResponse
    {
        public IEnumerable<SuctionModeView> SuctionModeViews { get; set; }

        public int TotalCount { get; set; }

        public bool success { get; set; }
    }
}
