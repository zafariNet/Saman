using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Services.Messaging.StoreCatalogService
{
    public class GetProductDeliverysResponse
    {
        public IEnumerable<ProductDeliveryView> ProductDeliveryViews { get; set; }
    }
}
