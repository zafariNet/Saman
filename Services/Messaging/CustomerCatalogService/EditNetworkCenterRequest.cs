using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class EditNetworkCenterRequest : AddNetworkCenterRequest
    {
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }
}
