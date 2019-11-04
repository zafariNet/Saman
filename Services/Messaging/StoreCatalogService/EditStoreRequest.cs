using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class EditStoreRequestOld : AddStoreRequestOld
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }

    public class EditStoreRequest : AddStoreRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }
    }
}
