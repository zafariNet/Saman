using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class SuctionModePageView : BasePageView
    {
        public SuctionModeView SuctionModeView { get; set; }

        public IEnumerable<SuctionModeView> SuctionModeViews { get; set; }
    }
}
