using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{

    public class AddProductRequestOld
    {
        public Guid ProductCategoryID { get; set; }
        public string ProductName { get; set; }
        public Int32 ProductCode { get; set; }
        public int UnitsInStock { get; set; }
        public bool Discontinued { get; set; }
        public string Note { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
    
    public class AddProductRequest
    {
        public string ProductName { get; set; }
        public Int32 ProductCode { get; set; }
        public string Note { get; set; }
        
    }
}
