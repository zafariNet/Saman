using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Employees
{
    public class EmployeeWithChildView : BaseView
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }
        public IList<EmployeeWithChildView> ChildEmployees { get; set; }
    }
}
