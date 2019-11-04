using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Infrastructure.Querying;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Controllers.Controllers
{
    public class SupportDeliverServiceController:BaseController
    {
        #region Declare

        private readonly ISupportService _supportService;

        private readonly ICustomerService _customerService;

        private readonly IEmployeeService _employeeService;

        private readonly ISupportDeliverServiceService _supportDeliverServiceService;

        #endregion

        #region ctor

        public SupportDeliverServiceController(ISupportDeliverServiceService supportDeliverServiceService,
            ISupportService supportService, ICustomerService customerService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _supportService = supportService;
            _customerService = customerService;
            _employeeService = employeeService;
            _supportDeliverServiceService = supportDeliverServiceService;
        }

        #endregion

        #region Read

        #region Read All

        public JsonResult SupportDeliverServices_ReadAll(int? pageSize, int? pageNumber,IList<FilterData> filter)
        {
            GetGeneralResponse<IEnumerable<SupportDeliverServiceView>> response=new GetGeneralResponse<IEnumerable<SupportDeliverServiceView>>();

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _supportDeliverServiceService.GetSupportDeliverServices(PageSize, PageNumber,filter);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read One

        public JsonResult SupportDeliverService_Read(Guid SupportID)
        {
            GetGeneralResponse<SupportDeliverServiceView> response=new GetGeneralResponse<SupportDeliverServiceView>();

            response = _supportDeliverServiceService.GetSupportDeliverService(SupportID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Insert

        public JsonResult SpportDeliverService_Insert(AddSupportDeliverServiceRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            //AddSupportDeliverServiceRequest request=new AddSupportDeliverServiceRequest();
            //request.AmountRecived = 200000;
            //request.DeliverDate = "1393/02/29";
            //request.TimeInput = "20:30";
            //request.TimeOutput = "22:30";
            //request.Comment = "توضیحات نصب در این قسمت وارد میشود";
            //request.SendNotificationToCustomer = true;
            //request.SupportID = Guid.Parse("AC57D46B-5139-4F03-B4D4-4A75B747CDCA");


            response = _supportDeliverServiceService.AddSeupportDeliverService(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult SupportDeliverService_Update(EditSupportDeliverServiceRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            response = _supportDeliverServiceService.EditSupportDeliverService(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult SupportDeliverService_Delete(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportDeliverServiceService.DeleteSupportDeliverService(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
