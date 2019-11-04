using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class AddProductCategoryRequest
    {
        public string ProductCategoryName { get; set; }
        public string Note { get; set; }
        public bool Discontinued { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
}
