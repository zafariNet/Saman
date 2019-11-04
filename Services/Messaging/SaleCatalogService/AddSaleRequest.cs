using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SaleCatalogService
{
    public class AddSaleRequest
    {
        public Guid CustomerID { get; set; }
        public Guid SaleID { get; set; }
        public string SaleNumber { get; set; }
        public Guid CreateEmployeeID { get; set; }

        public Guid MainSaleID { get; set; }

        public bool IsRollback { get; set; }

        public IEnumerable<AddProductSaleDetailRequest> AddProductSaleDetailRequests { get; set; }
        public IEnumerable<AddCreditSaleDetailRequest> AddCreditSaleDetailRequests { get; set; }
        public IEnumerable<AddUncreditSaleDetailRequest> AddUncreditSaleDetailRequests { get; set; }
    }
}
