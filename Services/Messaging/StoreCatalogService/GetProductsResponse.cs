using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Services.Messaging.StoreCatalogService
{
    public class GetProductsResponse
    {
        public IEnumerable<ProductView> ProductViews { get; set; }

        public int Count { get; set; }
    }
}
