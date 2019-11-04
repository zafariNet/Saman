#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Fiscals;
using Services.ViewModels.Employees;
#endregion

namespace Controllers.ViewModels.FiscalCatalog
{
    public class MoneyAccountDetailView : BasePageView
    {
        public MoneyAccountView MoneyAccountView { get; set; }
    }
}
