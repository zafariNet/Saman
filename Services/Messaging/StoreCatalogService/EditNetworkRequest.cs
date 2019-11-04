using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class EditNetworkRequestOld : AddNetworkRequestOld
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }

    public class EditNetworkRequest : AddNetworkRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }
    }
}
