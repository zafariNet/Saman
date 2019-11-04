#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Sales;
using Services.ViewModels.Customers;
using Services.ViewModels.Store;
using Controllers.ViewModels.StoreCatalog;
#endregion

namespace Controllers.ViewModels.SaleCatalog
{
    public class SaleDetailView : BasePageView
    {
        public SaleView SaleView { get; set; }

        public IEnumerable<ProductPriceView> ProductPriceViews { get; set; }

        public IEnumerable<CreditServiceView> CreditServiceViews { get; set; }

        public IEnumerable<UncreditServiceView> UncreditServiceViews { get; set; }

        public ProductPriceForCreate ProductPriceForCreate { get; set; }

        public CustomerView CustomerView { get; set; }
    }
}
