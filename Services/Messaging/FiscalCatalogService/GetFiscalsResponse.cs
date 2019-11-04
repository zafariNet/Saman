using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Fiscals;

namespace Services.Messaging.FiscalCatalogService
{
    public class GetFiscalsResponse
    {
        public IEnumerable<FiscalView> FiscalViews { get; set; }

        public int Count { get; set; }
    }
}
