using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Employees;

namespace Controllers.Controllers
{
    public class CourierEmployeeController : BaseController
    {
        #region Declare

        private readonly ICustomerService _customerService;

        private readonly IEmployeeService _employeeService;

        private readonly ICourierService _courierService;

        private readonly ICourierEmployeeService _courierEmployeeService;

        #endregion

        #region Ctor

        public CourierEmployeeController(ICustomerService customerService, IEmployeeService employeeService,
            ICourierService courierService, ICourierEmployeeService courierEmployeeService)
            : base(employeeService)
        {
            _customerService = customerService;
            _employeeService = employeeService;
            _courierService = courierService;
            _courierEmployeeService = courierEmployeeService;
        }

        #endregion

        #region Read All

        public JsonResult CourierEmployees_Read()
        {
            GetGeneralResponse<IEnumerable<CourierEmployeeView>> response=new GetGeneralResponse<IEnumerable<CourierEmployeeView>>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("CourierEmployee_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion


            response = _courierEmployeeService.GetCourierEmployees();

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Insert

        public JsonResult Courieremployee_Insert(AddCourierEmployeeRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("CourierEmployee_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            //AddCourierEmployeeRequest request=new AddCourierEmployeeRequest();
            //request.Address = "خیابان فرجام";
            //request.FirstName = "محمد";
            //request.LastName = "ظفری";
            //request.Mobile = "09190737487";
            //request.Phone = "44444444";

            response = _courierEmployeeService.AddCourierEmployee(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult CourierEployee_Update(EditCourierEmployeeRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("CourierEmployee_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _courierEmployeeService.EditCourierEmployee(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult CourierEployees_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("CourierEmployee_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _courierEmployeeService.DeleteCourierEmployee(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }

    
}
