using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Controllers.ViewModels.StoreCatalog
{
    public class ProductDeliveryHomePageView : BasePageView
    {
        public IEnumerable<ProductDeliveryView> ProductDeliveryViews { get; set; }
    }
}
