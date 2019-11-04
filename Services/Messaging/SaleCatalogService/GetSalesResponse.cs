using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Sales;

namespace Services.Messaging.SaleCatalogService
{
    public class GetSalesResponse
    {
        public IEnumerable<SaleView> SaleViews { get; set; }

        public IEnumerable<SimpleSaleView> SimpleSaleViews { get; set; }

        public int TotalCount { get; set; }
    }
}
