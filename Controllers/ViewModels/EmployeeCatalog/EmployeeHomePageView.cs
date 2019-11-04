using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Employees;

namespace Controllers.ViewModels.EmployeeCatalog
{
    public class EmployeeHomePageView : BasePageView
    {
        public IEnumerable<EmployeeView> EmployeeViews { get; set; }
    }
}
