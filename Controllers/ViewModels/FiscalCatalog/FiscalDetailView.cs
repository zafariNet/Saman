using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Fiscals;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.FiscalCatalog
{
    public class FiscalDetailView : BasePageView
    {
        public FiscalView FiscalView { get; set; }

        public IEnumerable<MoneyAccountView> MoneyAccountViews { get; set; }

        public CustomerView CustomerView { get; set; }
    }
}
