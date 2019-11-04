using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{

    public class EditProductRequestOld : AddProductRequestOld
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }

    public class EditProductRequest : AddProductRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }
        public bool Discontinued { get; set; }

     

    }
}
