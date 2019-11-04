using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Controllers.ViewModels.StoreCatalog
{
    public class CreditServiceDetailView : BasePageView
    {
        public CreditServiceView CreditServiceView { get; set; }

        public IEnumerable<NetworkView> NetworkViews { get; set; }
    }
}
