using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Infrastructure.Querying;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.Leadcatalogservice;
using Services.ViewModels.Leads;

namespace Controllers.Controllers
{
    public class LeadTitleTemplateController:BaseController
    {
        #region Declare

        private readonly IEmployeeService _employeeService;
        private readonly ILeadTitleTemplateService _leadTitleTemplateService;

        #endregion

        #region Ctor

        public LeadTitleTemplateController(IEmployeeService employeeService,ILeadTitleTemplateService leadTitleTemplateService):base(employeeService)
        {
            _leadTitleTemplateService = leadTitleTemplateService;
            _employeeService = employeeService;
        }

        #endregion

        #region Read All

        public JsonResult LeadTitleTemplate_ReadAll(int? pageSize, int? pageNumber, IList<FilterData> filter, string sort)
        {
            GetGeneralResponse<IEnumerable<LeadTitleTemplateView>> response=new GetGeneralResponse<IEnumerable<LeadTitleTemplateView>>();


            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            response = _leadTitleTemplateService.GetLeadTitleTemplates(PageSize,PageNumber,filter,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult LeadTitleTemplate_Insert(IEnumerable<AddLeadTitleTemplateRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("LeadTitleTemplate_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            //IList<AddLeadTitlTemplateRequest> list=new List<AddLeadTitlTemplateRequest>();
            //list.Add(new AddLeadTitlTemplateRequest()
            //{
            //    Title = "گفته با عمه ام مشورت میکنم"
            //});

            response = _leadTitleTemplateService.AddLeadTitleTemplate(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult LeadTitleTemplate_Update(IEnumerable<EditLeadTitleTemplateRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("LeadTitleTemplate_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

          //  IList<EditLeadTitleTemplateRequest> list=new List<EditLeadTitleTemplateRequest>();
          //list.Add(new EditLeadTitleTemplateRequest()
          //{
          //    ID = Guid.Parse("42e9bb42-d227-42f1-89e7-88927c214e0c"),
          //    RowVersion = 2,
          //    Title = "این بار با خاله اش مشورت میکند"
          //});
            response = _leadTitleTemplateService.EditLeadTitleTemplate(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult LeadTitleTemplate_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("LeadTitleTemplate_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _leadTitleTemplateService.DeleteLeadTitleTemplate(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
