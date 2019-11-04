using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Services.Messaging.StoreCatalogService
{
    public  class GetCreditServiceDeliverysResponse
    {
        public IEnumerable<CreditServiceDeliveryView> CreditServiceDeliveryViews { get; set; }
    }
}
