using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Controllers.ViewModels.StoreCatalog
{
    public class ProductPriceHomePageView : BasePageView
    {
        public IEnumerable<ProductPriceView> ProductPriceViews { get; set; }

        public ProductView ProductView { get; set; }
    }
}
