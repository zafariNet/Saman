using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddSuctionModeRequestOld
    {
        public string SuctionModeName { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }

    public class AddSuctionModeRequest
    {
        public string SuctionModeName { get; set; }
    }
}
