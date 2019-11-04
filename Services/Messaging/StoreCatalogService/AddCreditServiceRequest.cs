using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class AddCreditServiceRequestOld
    {
        public Guid NetworkID { get; set; }
        public string ServiceName { get; set; }
        public int CreditServiceCode { get; set; }
        public long UnitPrice { get; set; }
        public long PurchaseUnitPrice { get; set; }
        public long ResellerUnitPrice { get; set; }
        public long MaxDiscount { get; set; }
        public int Imposition { get; set; }
        public bool Discontinued { get; set; }
        public int ExpDays { get; set; }
        public string Note { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }

    public class AddCreditServiceRequest
    {
        public Guid NetworkID { get; set; }
        public string ServiceName { get; set; }
        public int CreditServiceCode { get; set; }
        public long UnitPrice { get; set; }
        public long PurchaseUnitPrice { get; set; }
        public long ResellerUnitPrice { get; set; }
        public long MaxDiscount { get; set; }
        public int Imposition { get; set; }
        public bool Discontinued { get; set; }
        public int ExpDays { get; set; }
        public string Note { get; set; }
        public long Comission { get; set; }
        public long Bonus { get; set; }
    }
}
