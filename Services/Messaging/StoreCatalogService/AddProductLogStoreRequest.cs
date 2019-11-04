using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class AddProductLogStoreRequest
    {
            public Guid ProductID { get; set; }
            public string LogDate { get; set; }
            public int DisplayUnitsIO { get; set; }
            public int IO { get; set; }
            public Int64 PurchaseUnitPrice { get; set; }
            public Int64 TotalLine { get; set; }
            public string PurchaseDate { get; set; }
            public string SellerName { get; set; }
            public string PurchaseBillNumber { get; set; }
            public bool Closed { get; set; }
            public string InputSerialNumber { get; set; }
            public string ProductSerialFrom { get; set; }
            public string ProductSerialTo { get; set; }
            public string Note { get; set; }
            public Guid StoreID { get; set; }
    }
}
