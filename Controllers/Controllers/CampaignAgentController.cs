using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Infrastructure.Querying;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;

namespace Controllers.Controllers
{
    public class CampaignAgentController:BaseController
    {

        #region Declar

        private readonly ICampaignAgentService _campaignAgentService;

        private readonly IEmployeeService _employeeService;

        #endregion

        #region ctor

        public CampaignAgentController(ICampaignAgentService campaignAgentService,IEmployeeService employeeService):base(employeeService)
        {
            _campaignAgentService = campaignAgentService;
            
        }
        #endregion

        #region Read

        #region Read All

        public JsonResult CampaignAgentes_Read(int? pageSize, int? pageNumber, IList<FilterData> filter, string sort)
        {
            GetGeneralResponse<IEnumerable<CampaignAgentView>> response=new GetGeneralResponse<IEnumerable<CampaignAgentView>>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("CampaignAgent_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion
            
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _campaignAgentService.GetCampaignAgentes(PageSize, PageNumber, filter, ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Insert

        public JsonResult CampaignAgentes_Insert(IEnumerable<AddCampaignAgentRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("CampaignAgent_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion


            #region Test

            //IList<AddCampaignAgentRequest> requests = new List<AddCampaignAgentRequest>();

            //requests.Add(new AddCampaignAgentRequest()
            //{
            //    CampaignAgentName = "محمد ظفری"
            //});

            #endregion

            response = _campaignAgentService.AddCampaignAgent(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult CampaignAgentes_Update(IEnumerable<EditCampaignAgentRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("CampaignAgent_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            #region test

            //IList<EditCampaignAgentRequest> requests = new List<EditCampaignAgentRequest>();

            //requests.Add(new EditCampaignAgentRequest()
            //{
            //    CampaignAgentName = "علی ذاکر اصفهانی",
            //    ID = Guid.Parse("725BE74B-B026-4C24-8A5C-1816D993BEFC"),
            //    RowVersion = 1
            //});


            #endregion

            response = _campaignAgentService.EditCampaignAgent(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult CampaignAgentes_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();


            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("CampaignAgent_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion


            #region Test


            //IList<DeleteRequest> requests = new List<DeleteRequest>();
            //requests.Add(new DeleteRequest()
            //{
            //    ID = Guid.Parse("725BE74B-B026-4C24-8A5C-1816D993BEFC")
            //});

            #endregion

            response = _campaignAgentService.DeleteCampaignAgenet(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
