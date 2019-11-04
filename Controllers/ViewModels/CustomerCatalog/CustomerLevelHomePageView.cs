using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class CustomerLevelHomePageView : BasePageView
    {
        public IEnumerable<CustomerLevelView> CustomerLevelViews { get; set; }
    }
}
