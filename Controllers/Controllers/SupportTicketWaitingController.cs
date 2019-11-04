using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Model.Support;
using Model.Support.Interfaces;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Controllers.Controllers
{
    public class SupportTicketWaitingController : BaseController
    {
        #region Declare

        private readonly ISupportService _supportService;

        private readonly ICustomerService _customerService;

        private readonly IEmployeeService _employeeService;

        private readonly ISupportTicketWaitingService _supportTicketWaitingService;

        #endregion

        #region ctor

        public SupportTicketWaitingController(ISupportTicketWaitingService supportTicketWaitingService,
            ISupportService supportService, ICustomerService customerService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _supportService = supportService;
            _customerService = customerService;
            _employeeService = employeeService;
            _supportTicketWaitingService = supportTicketWaitingService;
        }

        #endregion

        #region Read All

        public JsonResult SupportTicketWaitings_Read()
        {
            GetGeneralResponse<IEnumerable<SupportTicketWaitingView>> response=new GetGeneralResponse<IEnumerable<SupportTicketWaitingView>>();

            response = _supportTicketWaitingService.GetSupportTicketWaitings();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read One

        public JsonResult SupportTicketwaiting_Read(Guid SupportID)
        {
            GetGeneralResponse<SupportTicketWaitingView> response=new GetGeneralResponse<SupportTicketWaitingView>();

            response = _supportTicketWaitingService.GetSupportTicketWaiting(SupportID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult SupportTicketWaiting_Insert(AddSupportTicketWaitingRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            //AddSupportTicketWaitingRequest request=new AddSupportTicketWaitingRequest();

            //request.Comment = " این یک توضیح انتظار تیکت است";
            //request.DateOfPersenceDate = "1393/10/10";
            //request.InstallExpertID = Guid.Parse("12D942E9-9B2F-42A9-82D5-66D661FAC17D");
            //request.Selt = "120";
            //request.SendNotificationToCustomer = true;
            //request.Snr = "12/12";
            //request.SourceWireCheck = true;
            //request.SupportID = Guid.Parse("AC57D46B-5139-4F03-B4D4-4A75B747CDCA");
            //request.TicketSubject = "این یک عنوان است";
            //request.WireColor = "Red";

            response = _supportTicketWaitingService.AddSupportTicketWaiting(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult SupportTicketWaiting_Update(EditSupportTicketWaitingRequest request)
        {
           GeneralResponse response=new GeneralResponse();

            response = _supportTicketWaitingService.EditSupportTicketWaiting(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult SupportTicketWaiting_Delete(DeleteRequest request)
        {
            GeneralResponse respone=new GeneralResponse();

            respone = _supportTicketWaitingService.DeleteSupportTicketWaiting(request);

            return Json(respone, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}
