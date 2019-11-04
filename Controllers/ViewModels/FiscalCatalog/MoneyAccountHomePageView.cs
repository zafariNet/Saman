using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Fiscals;

namespace Controllers.ViewModels.FiscalCatalog
{
    public class MoneyAccountHomePageView : BasePageView
    {
        public IEnumerable<MoneyAccountView> MoneyAccountViews { get; set; }
    }
}
