using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class SuctionModeHomePageView : BasePageView
    {
        public IEnumerable<SuctionModeView> SuctionModeViews { get; set; }
    }
}
