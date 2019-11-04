using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Employees;
using Model.Employees;

namespace Services.Messaging.EmployeeCatalogService
{
    public class GetEmployeeResponse
    {
        public EmployeeView EmployeeView { get; set; }

        public Employee Employee { get; set; }
    }
}
