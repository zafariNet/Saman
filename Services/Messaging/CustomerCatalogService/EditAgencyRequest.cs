using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class EditAgencyRequestOld : AddAgencyRequestOld
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }

    public class EditAgencyRequest : AddAgencyRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }
    }
}

