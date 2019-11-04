using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class ConditionHomePageView : BasePageView
    {
        public IEnumerable<ConditionView> ConditionViews { get; set; }

        public int Count { get; set; }
    }
}
