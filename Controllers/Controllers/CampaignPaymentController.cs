using System;
using System.Collections;
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
    public class CampaignPaymentController:BaseController
    {
        #region Declar

        private readonly ICampaignAgentService _campaignAgentService;
        
        private readonly ICampaignPaymentService _campaignPaymentService;

        private readonly IEmployeeService _employeeService;

        #endregion

        #region Ctor

        public CampaignPaymentController(ICampaignAgentService campaignAgentService,
            ICampaignPaymentService campaignPaymentService, IEmployeeService employeeService) : base(employeeService)
        {
            _campaignAgentService = campaignAgentService;
            _campaignPaymentService = campaignPaymentService;
        }

        #endregion

        #region Read

        #region All

        public JsonResult CampaignPaymentes_Read(int? pageSize, int? pageNumber, IList<FilterData> filter, string sort,Guid? CampaignAgentID)
        {
            GetGeneralResponse<IEnumerable<CampaignPaymentView>> response=new GetGeneralResponse<IEnumerable<CampaignPaymentView>>();



            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("CampaignPayment_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            #region prepairing Filter

            IList<FilterData> _filter=new List<FilterData>();

            if (CampaignAgentID != null)
            {
                _filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eq",
                        type = "list",
                        value = new []{CampaignAgentID.ToString()}
                    },
                    field = "CampaignAgent.ID"

                });

                if (filter != null)
                {
                    foreach (var item in filter)
                    {
                        _filter.Add(item);
                    }
                }
            }

            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _campaignPaymentService.GetCampaignPayments(PageSize, PageNumber, _filter,
                ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read by Agent

        public JsonResult CampaignPaymentes_Read_ByAgent(int? pageSize, int pageNumber, Guid CampaignAgentID)
        {
            GetGeneralResponse<IEnumerable<CampaignPaymentView>> response = new GetGeneralResponse<IEnumerable<CampaignPaymentView>>();


            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("CampaignPayment_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _campaignPaymentService.GetCampaignPaymentsByAgent(PageSize, PageNumber, CampaignAgentID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Insert

        public JsonResult CampaignPayment_Insert(IEnumerable<AddCampignPaymentRequest> requests, Guid CampaignAgentID)
        {
            EmployeeView employee = GetEmployee();

            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("CampaignPayment_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion


            #region Test

            //AddCampignPaymentRequest request=new AddCampignPaymentRequest();
            //request.SuctionModeDetailID = Guid.Parse("3280190E-EBF0-4630-A09F-012834898054");
            //request.Amount = 1000000;
            //request.CampaignAgentID = Guid.Parse("9F55F75C-A75E-4B3C-9428-0963B3ACD54D");
            //request.PaymentDate = "1393/04/05 14:23:59";

            #endregion

            response = _campaignPaymentService.AddCampaignPayment(requests,CampaignAgentID, employee.ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult CampaignPayment_Update(IEnumerable<EditCampignPaymentRequest> requests, Guid CampaignAgentID)
        {
            GeneralResponse response=new GeneralResponse();
            EmployeeView employee = GetEmployee();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("CampaignPayment_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion


            response = _campaignPaymentService.EditCampaignPayment(requests, CampaignAgentID, employee.ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult CampaignPayment_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();


            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("CampaignPayment_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _campaignPaymentService.DeleteCampaignPayment(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
