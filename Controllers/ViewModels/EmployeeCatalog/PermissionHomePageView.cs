using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Employees;

namespace Controllers.ViewModels.EmployeeCatalog
{
    public class PermissionHomePageView : BasePageView
    {
        public IEnumerable<PermissionView> PermissionViews { get; set; }
    }
}
