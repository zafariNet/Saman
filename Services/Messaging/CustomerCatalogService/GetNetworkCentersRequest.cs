using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class GetNetworkCentersRequest
    {
        public Guid NetworkID { get; set; }

        public Guid CenterID { get; set; }

    }
}
