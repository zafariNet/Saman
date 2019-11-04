using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Infrastructure.Querying;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.SaleCatalogService;
using Services.ViewModels.Employees;
using Services.ViewModels.Sales;

namespace Controllers.Controllers
{
    public class CourierController:BaseController
    {

        #region Declare

        private readonly ICustomerService _customerService;

        private readonly IEmployeeService _employeeService;

        private readonly ICourierService _courierService;

        private readonly ICourierEmployeeService _courierEmployeeService;

        #endregion

        #region Ctor

        public CourierController(ICustomerService customerService, IEmployeeService employeeService,
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

        public JsonResult Couriers_read(int? pageSize, int? pageNumber, IList<FilterData> filter, string sort)
        {
            GetGeneralResponse<IEnumerable<CourierView>> response=new GetGeneralResponse<IEnumerable<CourierView>>();


            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            #region Access Check and retrive data

            EmployeeView employee = GetEmployee();
            bool hasPermission = employee.IsGuaranteed("Courier_CangeStatus");
            if (hasPermission)
            {
                response = _courierService.GetAllCouriers(PageSize, PageNumber, filter, ConvertJsonToObject(sort));
            }
            else
            {
                response = _courierService.GetAllCouriersByEmployee(PageSize, PageNumber, filter,
                    ConvertJsonToObject(sort), employee.ID);

            }
            #endregion



            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region By Customer

        public JsonResult Couriers_Read_ByCustomer(int? pageSize, int? pageNumber, Guid CustomerID)
        {
            GetGeneralResponse<IEnumerable<CourierView>> response = new GetGeneralResponse<IEnumerable<CourierView>>();


            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Courier_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _courierService.GetCstomerCouriers(PageSize, PageNumber, CustomerID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Insert


        public JsonResult Courier_Insert (AddCourierRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            EmployeeView employee = GetEmployee();

            #region Access Check

            bool hasPermission = employee.IsGuaranteed("Courier_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion



            response = _courierService.AddCourier(request, employee.ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult Courier_Update(EditCourierRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            //#region Access Check

            //bool hasPermission = GetEmployee().IsGuaranteed("Courier_Update");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            //#endregion

            response = _courierService.EditCourier(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult Courier_Delete(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();


            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Courier_CangeStatus");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _courierService.DeleteCourier(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
        
        #region Set Status

        public JsonResult Change_CourierStatus(Guid CourierID, string ExpertComment, int CourierStatuse,Guid? CourierEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            EmployeeView employee = GetEmployee();
            bool hasPermission = employee.IsGuaranteed("Courier_CangeStatus");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            Guid courierEmployeeID = CourierEmployeeID == null ? Guid.Empty : (Guid) CourierEmployeeID;
            response = _courierService.DoCourierAction(CourierID, CourierStatuse, ExpertComment, courierEmployeeID, employee.ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
