using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Fiscals;

namespace Controllers.ViewModels.FiscalCatalog
{
    public class FiscalPageView : BasePageView
    {
        public FiscalView FiscalView { get; set; }
        public IEnumerable<FiscalView> FiscalViews { get; set; }

        public MoneyAccountView MoneyAccountView { get; set; }
        public IEnumerable<MoneyAccountView> MoneyAccountViews { get; set; }
    }
}
