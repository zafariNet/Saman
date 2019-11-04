using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Employees;

namespace Controllers.ViewModels.EmployeeCatalog
{
    public class EmployeeDetailView : BasePageView
    {
        public EmployeeView EmployeeMainView { get; set; }

        public IEnumerable<GroupView> GroupViews { get; set; }

        public IEnumerable<EmployeeView> ParentEmployeeViews { get; set; }

        public IEnumerable<PermissionView> PermissionViews { get; set; }
    }
}
