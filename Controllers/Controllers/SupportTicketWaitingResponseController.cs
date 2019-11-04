using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Controllers.Controllers
{
    public class SupportTicketWaitingResponseController : BaseController
    {
        #region Declare

        private readonly ISupportService _supportService;

        private readonly ICustomerService _customerService;

        private readonly IEmployeeService _employeeService;

        private readonly ISupportTicketWaitingResponseService _supportTicketWaitingResponseService;

        #endregion

        #region ctor

        public SupportTicketWaitingResponseController(
            ISupportTicketWaitingResponseService supportTicketWaitingResponseService,
            ISupportService supportService, ICustomerService customerService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _supportService = supportService;
            _customerService = customerService;
            _employeeService = employeeService;
            _supportTicketWaitingResponseService = supportTicketWaitingResponseService;
        }

        #endregion

        #region Read All

        public JsonResult SupportTicketWaitingResponseWaitings_Read()
        {
            GetGeneralResponse<IEnumerable<SupportTicketWaitingResponseView>> response = new GetGeneralResponse<IEnumerable<SupportTicketWaitingResponseView>>();

            response = _supportTicketWaitingResponseService.GetSpportTicketWaitingsRespone();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read One

        public JsonResult SupportTicketWaitinResponse_Read(Guid SupportID)
        {
            GetGeneralResponse<SupportTicketWaitingResponseView> response=new GetGeneralResponse<SupportTicketWaitingResponseView>();

            response = _supportTicketWaitingResponseService.GetSpportTicketWaitingRespone(SupportID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult SupportTicketWaitingResponse_Insert(AddSupportTicketWaitingResponseRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            //AddSupportTicketWaitingResponseRequest request=new AddSupportTicketWaitingResponseRequest();
            //request.Comment = "توضیحات انتظار تیکت";
            //request.ResponsePossibilityDate = "1393/10/10";
            //request.SendNotificationToCustomer = true;
            //request.SendTicketDate = "1393/11/11";
            //request.SupportID = Guid.Parse("AC57D46B-5139-4F03-B4D4-4A75B747CDCA");
            //request.TicketNumber = "102030";

            response = _supportTicketWaitingResponseService.AddSpportTicketWaitingRespone(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult SupportTicketWaitingResponse_Update(EditSupportTicketWaitingResponseRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportTicketWaitingResponseService.EditSpportTicketWaitingRespone(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult SupportTicketWaitinResponse_Delete(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportTicketWaitingResponseService.DeleteSupportTicketWaitingResponse(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
