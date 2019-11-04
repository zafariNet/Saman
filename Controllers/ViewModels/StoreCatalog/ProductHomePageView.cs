using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Controllers.ViewModels.StoreCatalog
{
    public class ProductHomePageView : BasePageView
    {
        public IEnumerable<ProductView> ProductViews { get; set; }

        public int Count { get; set; }
    }
}
