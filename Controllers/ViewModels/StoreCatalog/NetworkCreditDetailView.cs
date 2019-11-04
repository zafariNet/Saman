using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;
using Services.ViewModels.Fiscals;

namespace Controllers.ViewModels.StoreCatalog
{
    public class NetworkCreditDetailView : BasePageView
    {
        public NetworkCreditView NetworkCreditView { get; set; }

        public IEnumerable<MoneyAccountView> MoneyAccountViews { get; set; }
    }
}
