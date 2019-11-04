using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Model.Employees;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;

namespace Controllers.Controllers
{
    public class LocalPhoneStoreEmployeeController:BaseController
    {

        #region Declare

        private readonly IEmployeeService _employeeService;

        private readonly ILocalPhoneStoreEmployeeService _localPhoneStoreEmployeeService;

        #endregion

        #region Ctor

        public LocalPhoneStoreEmployeeController(IEmployeeService employeeService,
            ILocalPhoneStoreEmployeeService localPhoneStoreEmployeeService) : base(employeeService)
        {
            _employeeService = employeeService;
            _localPhoneStoreEmployeeService = localPhoneStoreEmployeeService;
        }

        #endregion

        #region Read All

        public JsonResult LocalPhoneStoreEmployees_read(int? pageSize, int? pageNumber, IList<FilterData> filter,
            string sort)
        {
            GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>> response=new GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>>();

            

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _localPhoneStoreEmployeeService.GetAllLocalPhoneStoreEmployee(PageSize, PageNumber, filter,
                ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read By Employee

        public JsonResult LocalPhoneStoreEmployee_Read_ByEmployee(Guid EmployeeID)
        {
            GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>> response=new GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>>();

            response = _localPhoneStoreEmployeeService.GetLocalPhoneStoreEmployeeByEmployee(EmployeeID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LocalPhoneStoreEmployee_OwnEployee()
        {
            GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>> response = new GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>>();

            response = _localPhoneStoreEmployeeService.GetLocalPhoneStoreEmployeeByEmployee(GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Add

        public JsonResult LocalPhoneStoreEmployees_Insert(IEnumerable<AddLocalPhoneStoreEmployeeRequest> requests,
            Guid EmployeeID)
        {

            //IList<AddLocalPhoneStoreEmployeeRequest> req=new List<AddLocalPhoneStoreEmployeeRequest>();

            //req.Add(new AddLocalPhoneStoreEmployeeRequest()
            //{
            //    DangerousRing = 10,
            //    DangerousSeconds = 20,
            //    LocalPhoneStoreID = Guid.Parse("BA0A7CE1-5A03-4EB0-9E4D-006EB55AD036"),
            //    SendSmsToOffLineUserOnDangerous = true,
            //    SendSmsToOnLineUserOnDangerous = true,
            //    //OwnerEmployeeID = Guid.Parse("12D942E9-9B2F-42A9-82D5-66D661FAC17D"),
            //    SmsText = "سلام سوسیس"
            //});

            GeneralResponse response=new GeneralResponse();

            response = _localPhoneStoreEmployeeService.AddLocalPhoneStoreEmployee(requests, EmployeeID,
                GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Remove

        public JsonResult LocalPhoneStoreEmployee_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            response = _localPhoneStoreEmployeeService.DeleteLocalPhoneStoreEmployee(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
