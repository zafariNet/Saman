using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Controllers.ViewModels.StoreCatalog
{
    public class StoreProductHomePageView : BasePageView
    {
        public IEnumerable<StoreProductView> StoreProductViews { get; set; }

        public StoreProductView StoreProductView { get; set; }

        public StoreView StoreView { get; set; }

        public int Count { get; set; }
    }
}
