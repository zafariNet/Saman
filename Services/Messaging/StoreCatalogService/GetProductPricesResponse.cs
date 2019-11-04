using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Services.Messaging.StoreCatalogService
{
    public class GetProductPricesResponse
    {
        public IEnumerable<ProductPriceView> ProductPriceViews { get; set; }
    }
}
