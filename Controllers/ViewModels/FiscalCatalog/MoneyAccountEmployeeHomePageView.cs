using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Fiscals;

namespace Controllers.ViewModels.FiscalCatalog
{
    public class MoneyAccountEmployeeHomePageView : BasePageView
    {
        public IEnumerable<MoneyAccountEmployeeView> MoneyAccountEmployeeViews { get; set; }
    }
}
