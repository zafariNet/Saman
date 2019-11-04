using Model.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SaleCatalogService
{
    public class AddUncreditSaleDetailRequest:AddSaleDetailBaseRequest
    {
        public Guid UncreditServiceID { get; set; }

        public Guid MainUncreditSaleDetailID { get; set; }

        public long CanRollbackDiscountPrice { get; set; }
        public long CanRollbackImpositionPrice { get; set; }

        public bool IsDeliverdBefor { get; set; }

        

        public SaleDetailStatus Status { get; set; }
    }
}
