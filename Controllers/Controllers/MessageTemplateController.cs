using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Employees;

namespace Controllers.Controllers
{
    public class MessageTemplateController:BaseController
    {
        #region Declare

        private readonly IEmployeeService _employeeService;
        private readonly IMessageTemplateService _MessageTemplateService;

        #endregion

        #region ctor

        public MessageTemplateController(IEmployeeService employeeService,IMessageTemplateService messageTemplateService):base(employeeService)
        {
            _MessageTemplateService = messageTemplateService;
            _employeeService = employeeService;
        }

        #endregion

        #region Read

        public JsonResult MessageTemplate_Read(int? MessageType, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<MessageTemplateView>> response=new GetGeneralResponse<IEnumerable<MessageTemplateView>>();


            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MessageTemplate_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion


            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _MessageTemplateService.GetMessageTemplates(MessageType, PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult MessageTemplate_Insert(AddMessageTemplateRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            //#region Check Access
            //bool hasPermission = GetEmployee().IsGuaranteed("MessageTemplate_Insert");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
            //#endregion

            response = _MessageTemplateService.AddMessageTemplates(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// افزودن متن پیامک
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="MessageSmsTemplateText"></param>
        /// <returns></returns>
        public JsonResult MessageTemplate_Sms_Insert(Guid ID, string MessageSmsTemplateText)
        {
            GeneralResponse response=new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MessageTemplate_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _MessageTemplateService.AddSmsToMessageTemplate(ID, MessageSmsTemplateText, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// افزودن متن ایمیل
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="MessageSmsTemplateText"></param>
        /// <returns></returns>
        public JsonResult MessageTemplate_Email_Insert(Guid ID, string MessageEmailTemplateText)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MessageTemplate_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _MessageTemplateService.AddEmailToMessageTemplate(ID, MessageEmailTemplateText, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult MessageTemplate_Update(IEnumerable<EditeMessageTemplateRequest> request)
        {
            GeneralResponse response=new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MessageTemplate_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _MessageTemplateService.EditMessageTemplates(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete

        public JsonResult MessageTemplate_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();


            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MessageTemplate_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _MessageTemplateService.DeleteMessageTemplate(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
