using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Employees;

namespace Controllers.ViewModels
{
    public abstract class BasePageView
    {
        // For showing logon information at top of the page

        public EmployeeView EmployeeView { get; set; }
    }
}
