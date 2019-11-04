using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Fiscals;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.FiscalCatalog
{
    public class FiscalHomePageView : BasePageView
    {
        public IEnumerable<FiscalView> FiscalViews { get; set; }

        public int Count { get; set; }

        public CustomerView CustomerView { get; set; }
    }
}
