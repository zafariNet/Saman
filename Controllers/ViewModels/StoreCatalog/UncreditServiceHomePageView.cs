using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Controllers.ViewModels.StoreCatalog
{
    public class UncreditServiceHomePageView : BasePageView
    {
        public IEnumerable<UncreditServiceView> UncreditServiceViews { get; set; }
    }
}
