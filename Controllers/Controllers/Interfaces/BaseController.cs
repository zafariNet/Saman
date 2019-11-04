#region Usings
using System;
using System.Web.Mvc;
using Controllers.ViewModels.CustomerCatalog;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Customers;
using Services.ViewModels.Employees;
using System.Collections.Generic;
#endregion

namespace Controllers.Controllers.Interfaces
{
    public interface IBaseController
    {
        EmployeeView GetEmployee(string EmpID);
        JsonResult CurrentEmployee();
        JsonResult GetEmployees();
        EmployeeView GetEmployee();
        CustomerView GetCustomer(string customerID);
        CustomerView GetCustomer(Guid customerID);
        JsonResult FindCustomers(QuickSearchRequest request);
    }
}
