using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Sales;

namespace Controllers.ViewModels.SaleCatalog
{
    public class SaleHomePageView : BasePageView
    {
        public IEnumerable<SaleView> SaleViews { get; set; }
    }
}
