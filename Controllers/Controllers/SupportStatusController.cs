using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Mvc;
using Model.Support;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Controllers.Controllers
{
    public class SupportStatusController:BaseController
    {

        #region declare

        private readonly IEmployeeService _employeeService;
        private readonly ISupportStatusService _supportStatusService;
        private readonly ISupportStatusRelationService _supportStatusRelationService;

        #endregion

        #region ctor

        public SupportStatusController(IEmployeeService employeeService, ISupportStatusService supportStatusService,
            ISupportStatusRelationService supportStatusRelationService)
            : base(employeeService)
        {
            _supportStatusService = supportStatusService;
            _supportStatusRelationService = supportStatusRelationService;
        }

        #endregion

        #region Read

        #region All

        public JsonResult SupportStatuses_read(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<SupportStatusView>> response=new GetGeneralResponse<IEnumerable<SupportStatusView>>();

            
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _supportStatusService.GetAllSupportStatuses(PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region One

        public JsonResult SupportStatus_Read(Guid SupportStatusID)
        {
            GetGeneralResponse<SupportStatusView> response=new GetGeneralResponse<SupportStatusView>();

            response = _supportStatusService.GetAllSupportStatus(SupportStatusID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region First Statuses

        public JsonResult SupportStatus_FirstStatuses_Read()
        {
            GetGeneralResponse<IEnumerable<SupportStatusView>> response=new GetGeneralResponse<IEnumerable<SupportStatusView>>();

            response = _supportStatusService.GetFirstSupportStatuses();

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #endregion

        #region Insert

        public JsonResult SupportStatus_Insert(AddSupportStatusRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            response = _supportStatusService.AddSupportStatus(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update

        public JsonResult SupportStatus_Update(IEnumerable<EditSupportStatusRequest> requests )
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportStatusService.EditSupportStatus(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult SupportStatuses_Delete(IEnumerable<DeleteRequest> requests)

        {
            GeneralResponse response=new GeneralResponse();

            //response = _supportStatusService.DeleteSupportStatuses(requests);

            response.ErrorMessages.Add("امکان حذف وضعیت پشتیبانی وجود ندارد");

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Update SMS and Email

        public JsonResult SupportStatus_Update_Sms(Guid SupportStatusID, string SmsText)
        {
            GeneralResponse response = new GeneralResponse();

            response = _supportStatusService.EditSms(SupportStatusID, SmsText, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SupportStatus_Update_Email(Guid SupportStatusID, string EmailText)
        {
            GeneralResponse response = new GeneralResponse();

            response = _supportStatusService.EditEmail(SupportStatusID, EmailText, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
