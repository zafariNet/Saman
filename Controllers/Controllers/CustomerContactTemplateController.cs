using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Customers;

namespace Controllers.Controllers
{
    public class CustomerContactTemplateController:BaseController
    {
        #region Private Members

        private readonly IEmployeeService _employeeService;
        private readonly ICustomerContactTemplateService _customerContactTemplateService;

        #endregion

        #region Ctor

        public CustomerContactTemplateController(IEmployeeService employeeService,ICustomerContactTemplateService customerContactTemplateService)
            : base(employeeService)
        {
            _employeeService = employeeService;
            _customerContactTemplateService = customerContactTemplateService;
        }

        #endregion

        #region Read

        public JsonResult CustomerContactTemplate_ReadAll(int? pageSize, int? pageNumber)
        {
            var response = new GetGeneralResponse<IEnumerable<CustomerContactTemplateView>>();

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _customerContactTemplateService.GetAll(PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CustomerContactTemplate_ReadByGroup()
        {
            var response = new GetGeneralResponse<IEnumerable<CustomerContactTemplateView>>();


            response = _customerContactTemplateService.GetAllByGroup(GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult CustomerContactTemplate_Insert(IEnumerable<AddCustomerContactTemplateRequest> requests)
        {
            var response = new GeneralResponse();

            response = _customerContactTemplateService.AddCustomerContactTemplate(requests, GetEmployee().ID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult CustomerContactTemplate_Update(IEnumerable<EditCustomerContactTemplateRequest> requests)
        {
            var response = new GeneralResponse();

            response = _customerContactTemplateService.EditCustomerContactTemplate(requests, GetEmployee().ID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult CustomerContactTemplate_Delete(IEnumerable<DeleteRequest> requests)
        {
            var response = new GeneralResponse();
            response = _customerContactTemplateService.DeleteCustomerCOntactTemplate(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
