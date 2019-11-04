using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Sales;

namespace Services.Messaging.SaleCatalogService
{
    public class CloseSaleRequest
    {
        public Guid SaleID { get; set; }

        public Guid CloseEmployeeID { get; set; }

        public int RowVersion { get; set; }
    }
}
