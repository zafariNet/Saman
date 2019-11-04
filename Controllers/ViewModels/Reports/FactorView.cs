using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Sales;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.Reports
{
    public class FactorView:BasePageView
    {
        public SaleView SaleView { get; set; }

        public IEnumerable<SaleDetailReportView> SaleDetailViews { get; set; }

        public CustomerView CustomerView { get; set; }
    }
}
