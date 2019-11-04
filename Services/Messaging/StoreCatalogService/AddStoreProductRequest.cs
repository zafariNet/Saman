using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class AddStoreProductRequest
    {
        public Guid ProductID { get; set; }
        public Guid StoreID { get; set; }
        public int UnitsInStock { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
}
