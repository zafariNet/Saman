using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Fiscals;

namespace Services.Messaging.FiscalCatalogService
{
    public class GetMoneyAccountEmployeesResponse
    {
        public IEnumerable<MoneyAccountEmployeeView> MoneyAccountEmployeeViews { get; set; }
    }
}
