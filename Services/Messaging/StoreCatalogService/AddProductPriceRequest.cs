using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class AddProductPriceRequestOld
    {
        public Guid ProductID { get; set; }
        public string ProductPriceTitle { get; set; }
        public long UnitPrice { get; set; }
        
        public long MaxDiscount { get; set; }
        public long Imposition { get; set; }
        public bool Discontinued { get; set; }
        public string Note { get; set; }
        public int? SortOrder { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }

    public class AddProductPriceRequest
    {
        public Guid ProductID { get; set; }
        public string ProductPriceTitle { get; set; }
        public long UnitPrice { get; set; }
        public int ProductPriceCode { get; set; }
        public long MaxDiscount { get; set; }
        public long Imposition { get; set; }
        public bool Discontinued { get; set; }
        public string Note { get; set; }
        public int? SortOrder { get; set; }
        public long Comission { get; set; }
        public long Bonus { get; set; }
    }
}
