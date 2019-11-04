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
using Controllers.Controllers.Interfaces;
using System.Web.Script.Serialization;
using Infrastructure.Querying;
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class BaseController : Controller, IBaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly ICustomerService _customerService;
        #endregion

        #region Ctor

        public BaseController(IEmployeeService employeeService, ICustomerService customerService)
        {
            _employeeService = employeeService;
            _customerService = customerService;
        }

        public BaseController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        #endregion

        #region Get Employee

        public EmployeeView GetEmployee(string EmpID)
        {
            GetRequest request = new GetRequest();
            GetEmployeeResponse response=new GetEmployeeResponse();
            request.ID = Guid.Parse(EmpID);
            if (Session[EmpID] == null)
            {
                response = _employeeService.GetEmployee(request);
                Session[EmpID] = response;
            }
            else
            {
                response = (GetEmployeeResponse)Session[EmpID];
                
            }

            EmployeeView employeeView = new EmployeeView();
            employeeView = response.EmployeeView;

            return employeeView;
        }

        public EmployeeView GetSimpleEmployee(string EmpID)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(EmpID);

            GetEmployeeResponse response = _employeeService.GetSimpleEmployee(request);

            EmployeeView employeeView = new EmployeeView();
            employeeView = response.EmployeeView;

            return employeeView;
        }

        public EmployeeView GetSimpleEmployee()
        {
            EmployeeView _employeeView = new EmployeeView();
            if (User != null && User.Identity.Name != "")
            {
                _employeeView = GetSimpleEmployee(User.Identity.Name);
            }

            return _employeeView;
        }

        public EmployeeView GetEmployee()
        {
            EmployeeView _employeeView = new EmployeeView();
            if (User != null && User.Identity.Name != "")
            {
                    _employeeView = GetEmployee(User.Identity.Name);
            }

            return _employeeView;
        }

        public JsonResult CurrentEmployee()
        {
            EmployeeView employeeView = GetEmployee();

            GetGeneralResponse<EmployeeView> response = new GetGeneralResponse<EmployeeView>();

            response.data = employeeView;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployees()
        {
            IEnumerable<EmployeeView> employees = _employeeService.GetEmployees().EmployeeViews;


            return Json(employees, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeesNoPermission()
        {
            
            IEnumerable<SimpleEmployeeView> employees = _employeeService._GetEmployees().data;
            IList<EmployeeView> _employees = new List<EmployeeView>();
            //foreach (var item in employees)
            //{
            //    item.Permissions = null;
            //    _employees.Add(item);
            //}
            //employees = _employees;

            return Json(employees, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Customer

        public CustomerView GetCustomer(string customerID)
        {
            return GetCustomer(Guid.Parse(customerID));
        }

        public CustomerView GetCustomer(Guid customerID)
        {
            GetRequest request = new GetRequest() { ID = customerID };
            GetCustomerResponse cus = _customerService.GetCustomer(request);
            return cus.CustomerView;
        }

        public JsonResult FindCustomers(QuickSearchRequest request)
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response = _customerService.FindCustomers(request, GetEmployee().ID);

            // فعلا به فرمت قدیم پاس می دهیم
            GetCustomersResponse cresponse = new GetCustomersResponse()
            {
                CustomerViews = response.data,
                success = true,
                TotalCount = response.totalCount
            };

            return Json(cresponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public bool IsPermited(string methodName)
        {
            return true;
            //return GetEmployee().IsGuaranteed(methodName);
        }
        /// <summary>
        /// تبدیل آبجکت جیسون به کلاس مربوط به سورتینگ
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public IList<Sort> ConvertJsonToObject(string jsonString)
        {
            IList<Sort> response = new List<Sort>();
            if (jsonString != null)
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                UiSort[] sortList = jsonSerializer.Deserialize<UiSort[]>(jsonString);

                foreach (var item in sortList)
                {
                    Sort sort = new Sort();
                    if (item.direction == "DESC")
                        sort.Asc = false;
                    else
                        sort.Asc = true;
                    //if(item.property=="NetworkName")
                    //    sort.SortColumn = "Network.NetworkName";
                    //else
                    sort.SortColumn = item.property;

                    response.Add(sort);
                }
                
            }
            
            return response;
        }

        public IList<FilterModel> ConvertJsonToAdvancedFiler(string jsonString)
        {
            IList<FilterModel> response = new List<FilterModel>();
            if (jsonString != null)
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                FilterModel[] sortList = jsonSerializer.Deserialize<FilterModel[]>(jsonString);

            }
            return response;
        }
        

    }


}
