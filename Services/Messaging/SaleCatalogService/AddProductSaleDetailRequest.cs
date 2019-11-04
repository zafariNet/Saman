using Model.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SaleCatalogService
{
    public class AddProductSaleDetailRequest : AddSaleDetailBaseRequest
    {
        public Guid ProductPriceID { get; set; }

        public Guid MainProductSaleDetailID { get; set; }

        public long CanRollbackDiscountPrice { get; set; }
        public long CanRollbackImpositionPrice { get; set; }

        public bool IsDeliverdBefor { get; set; }
        public SaleDetailStatus Status { get; set; }
    }
}
