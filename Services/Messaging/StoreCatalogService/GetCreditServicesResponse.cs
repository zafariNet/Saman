using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Services.Messaging.StoreCatalogService
{
    public class GetCreditServicesResponse
    {
        public IEnumerable<CreditServiceView> CreditServiceViews { get; set; }
    }
}
