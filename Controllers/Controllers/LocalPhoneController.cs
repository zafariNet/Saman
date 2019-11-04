using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Infrastructure.Querying;
using NHibernate.Mapping;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.Messaging.ReportCatalogService;
using Services.ViewModels.Employees;

namespace Controllers.Controllers
{
    public class LocalPhoneController:BaseController
    {

        #region Declare

        private readonly IEmployeeService _employeeService;

        private readonly ILocalPhoneService _localPhoneService;

        #endregion


        #region Ctor

        public LocalPhoneController(IEmployeeService emploeeService,ILocalPhoneService localPhoneService):base(emploeeService)
        {
            _employeeService = emploeeService;
            _localPhoneService = localPhoneService;
        }

        #endregion

        #region Read All

        public JsonResult LocalPhones_Read(int? pageSize, int? pageNumber, List<FilterData> filter, string sort)
        {
            GetGeneralResponse<IEnumerable<LocalPhoneView>> response=new GetGeneralResponse<IEnumerable<LocalPhoneView>>();

            int PageNumber = pageNumber == null ? -1 : (int) pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;


            response = _localPhoneService.GetLocalPhones(PageSize, PageNumber, filter, ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LocalPhones_read_ByEmployee(Guid EmployeeID)
        {
            GetGeneralResponse<IEnumerable<LocalPhoneView>> response=new GetGeneralResponse<IEnumerable<LocalPhoneView>>();
            response = _localPhoneService.GetLocalPhonesByEmployee(EmployeeID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult LocalPhones_Insert(IEnumerable<AddLocalPhoneRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            response = _localPhoneService.AddLocalPhone(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult LocalPhones_Update(IEnumerable<EditLocalPhoneRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            response = _localPhoneService.EditLocalPhone(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult LocalPhones_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            response = _localPhoneService.DeleteLocalPhone(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
