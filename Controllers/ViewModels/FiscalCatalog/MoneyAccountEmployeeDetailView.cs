using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Fiscals;
using Services.ViewModels.Employees;

namespace Controllers.ViewModels.FiscalCatalog
{
    public class MoneyAccountEmployeeDetailView : BasePageView
    {
        public MoneyAccountEmployeeView MoneyAccountEmployeeView { get; set; }

        public IEnumerable<MoneyAccountEmployeeView> MoneyAccountEmployeeViews { get; set; }

        public MoneyAccountView MoneyAccountView { get; set; }

        public IEnumerable<EmployeeView> EmployeeViews { get; set; }

        public EmployeeView EmployeeViewForInsert { get; set; } // neede for Insert
    }
}
