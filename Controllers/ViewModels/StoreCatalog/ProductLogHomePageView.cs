using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Controllers.ViewModels.StoreCatalog
{
    public class ProductLogHomePageView : BasePageView
    {
        public IEnumerable<ProductLogView> ProductLogViews { get; set; }

        public ProductView ProductView { get; set; }
    }
}
