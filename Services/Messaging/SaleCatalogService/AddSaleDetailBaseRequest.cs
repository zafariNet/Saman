using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SaleCatalogService
{
    public class AddSaleDetailBaseRequest
    {
        public Guid ID { get; set; }
        public long UnitPrice { get; set; }
        public int Units { get; set; }
        public long Discount { get; set; }
        public long Imposition { get; set; }
        public Guid CreateEmployeeID { get; set; }
        public long LineDiscount { get; set; }
        public long LineImposition { get; set; }
        public long LineTotalWithoutDiscountAndImposition { get; set; }
        public string RollbackNote { get; set; }
        public long RollbackPrice { get; set; }
        public bool Rollbacked { get; set; }
        public long LineTotal { get; set; }
        public string RollbackDate { get; set; }
        public bool IsRollbackDetail { get; set; }
        public bool HasCourier { get; set; }    

    }
}
