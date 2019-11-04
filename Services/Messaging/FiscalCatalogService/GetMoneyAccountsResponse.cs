using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Fiscals;

namespace Services.Messaging.FiscalCatalogService
{
    public class GetMoneyAccountsResponse
    {
        public IEnumerable<MoneyAccountView> MoneyAccountViews { get; set; }
    }
}
