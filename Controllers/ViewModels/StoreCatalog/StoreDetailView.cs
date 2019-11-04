using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;
using Services.ViewModels.Employees;

namespace Controllers.ViewModels.StoreCatalog
{
    public class StoreDetailView : BasePageView
    {
        public StoreView StoreView { get; set; }

        public IEnumerable<EmployeeView> EmployeeViews { get; set; }
    }
}
