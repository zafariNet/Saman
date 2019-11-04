using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class EditUncreditServiceRequestOld : AddUncreditServiceRequestOld
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }

    public class EditUncreditServiceRequest : AddUncreditServiceRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

    }
}
