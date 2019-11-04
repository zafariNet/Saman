using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class AddNetworkRequestOld
    {
        public string NetworkName { get; set; }
        public string Note { get; set; }
        public bool DeliverWhenCreditLow { get; set; }
        public bool Discontinued { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }

    public class AddNetworkRequest
    {
        public string Alias { get; set; }
        public string NetworkName { get; set; }
        public string Note { get; set; }
        public bool DeliverWhenCreditLow { get; set; }
        public bool Discontinued { get; set; }
        
    }
}
