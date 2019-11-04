using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class SpecialNumberPageView : BasePageView
    {
        public SpecialNumberView SpecialNumberView { get; set; }

        public IEnumerable<SpecialNumberView> SpecialNumberViews { get; set; }
    }
}
