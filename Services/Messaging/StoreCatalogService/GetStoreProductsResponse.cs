using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Services.Messaging.StoreCatalogService
{
    public class GetStoreProductsResponse
    {
        public IEnumerable<StoreProductView> StoreProductViews { get; set; }

        public int Count { get; set; }
    }
}
