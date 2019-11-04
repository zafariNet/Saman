using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Controllers.ViewModels.StoreCatalog
{
    public class CreditServiceHomePageView : BasePageView
    {
        public IEnumerable<CreditServiceView> CreditServiceViews { get; set; }
    }
}
