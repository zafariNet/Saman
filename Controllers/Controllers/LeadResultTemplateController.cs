using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Infrastructure.Querying;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.Leadcatalogservice;
using Services.ViewModels.Leads;

namespace Controllers.Controllers
{
    public class LeadResultTemplateController : BaseController
    {
        #region Declare

        private readonly IEmployeeService _employeeService;
        private readonly ILeadResultTemplateService _leadResultTemplateService;

        #endregion

        #region Ctor

        public LeadResultTemplateController(IEmployeeService employeeService,
            ILeadResultTemplateService leadResultTemplateService) : base(employeeService)
        {
            _employeeService = employeeService;
            _leadResultTemplateService = leadResultTemplateService;
        }

        #endregion

        #region GetAll

        public JsonResult LeadResultTemplates_Read(int? pageSize, int? pageNumber,IList<FilterData> filter,string sort)
        {
            GetGeneralResponse<IEnumerable<LeadResultTemplateView>> response =
                new GetGeneralResponse<IEnumerable<LeadResultTemplateView>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("LeadResultTemplate_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            
            int PageNumber = pageNumber == null ? -1 : (int) pageNumber;
            int PageSize = pageSize == null ? -1 : (int) pageSize;

            response = _leadResultTemplateService.GetLeadResultTemplates(PageSize, PageNumber,filter,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Insert

        public JsonResult LeadResultTemplate_Insert(IEnumerable<AddLeadResultTemplateRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            response = _leadResultTemplateService.AddLeadResultTemplate(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region update

        public JsonResult LeadResulTemplates_Update(IEnumerable<EditLeadResultTemplateRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("LeadResultTemplate_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _leadResultTemplateService.EditLeadResultTemplate(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult LeadResultTemplates_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("LeadResultTemplate_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _leadResultTemplateService.DeleteLeadResultTemplate(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
