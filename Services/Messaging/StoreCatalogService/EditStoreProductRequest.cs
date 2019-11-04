using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class EditStoreProductRequest : AddStoreProductRequest
    {
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }
}
