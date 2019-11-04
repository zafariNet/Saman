using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Controllers.ViewModels.StoreCatalog
{
    public class ProductCategoryHomePageView : BasePageView
    {
        public IEnumerable<ProductCategoryView> ProductCategoryViews { get; set; }
    }
}
