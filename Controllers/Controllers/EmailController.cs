#region Usings

using System;
using System.IO;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml.Serialization;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Customers;
using System.Net.Configuration;
using System.Collections.Generic;
using Controllers.ViewModels.CustomerCatalog;

#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class EmailController : BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly IEmailService _emailService;

        #endregion

        #region ctor

        public EmailController(IEmployeeService employeeService, IEmailService emailService)
            : base(employeeService)
        {
            this._emailService = emailService;
            this._employeeService = employeeService;
        }

        #endregion

        #region Old Methods

        #region Index

        public ActionResult Index()
        {
            EmailHomePageView emailHomePageView = new EmailHomePageView();
            emailHomePageView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Email_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(emailHomePageView);
            }
            #endregion

            
            emailHomePageView.EmailViews = this._emailService.GetEmails().EmailViews;

            return View(emailHomePageView);
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            EmailDetailView emailDetailView = new EmailDetailView();
            emailDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Email_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(emailDetailView);
            }
            #endregion

            return View(emailDetailView);
        }

        [HttpPost]
        public ActionResult Create(EmailDetailView emailDetailView)
        {

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Email_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(emailDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddEmailRequest request = new AddEmailRequest();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.Body = emailDetailView.EmailView.Body;
                    request.CustomerID = emailDetailView.EmailView.CustomerID;
                    request.Sent = emailDetailView.EmailView.Sent;
                    request.Subject = emailDetailView.EmailView.Subject;
                    request.Note = emailDetailView.EmailView.Note;

                    GeneralResponse response = _emailService.AddEmail(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(emailDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(emailDetailView);
                }

            return View(emailDetailView);
        }

        #endregion

        #region Edit

        public ActionResult Edit(string id)
        {

            EmailDetailView emailDetailView = new EmailDetailView();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Email_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(emailDetailView);
            }
            #endregion

            emailDetailView.EmailView = this.GetEmailView(id);
            //emailDetailView.EmployeeView = GetEmployee();

            return View(emailDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, EmailDetailView emailDetailView)
        {
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Email_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(emailDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditEmailRequest request = new EditEmailRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.Body = emailDetailView.EmailView.Body;
                    request.CustomerID = emailDetailView.EmailView.CustomerID;
                    request.Sent = emailDetailView.EmailView.Sent;
                    request.Subject = emailDetailView.EmailView.Subject;
                    request.Note = emailDetailView.EmailView.Note;
                    request.RowVersion = emailDetailView.EmailView.RowVersion;

                    GeneralResponse response = this._emailService.EditEmail(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(emailDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(emailDetailView);
                }

            return View(emailDetailView);
        }

        #endregion

        #region Details

        public ActionResult Details(string id)
        {
            EmailDetailView emailDetailView = new EmailDetailView();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Email_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(emailDetailView);
            }
            #endregion

            EmailView emailView = this.GetEmailView(id);

            emailDetailView.EmailView = emailView;
            // emailDetailView.EmployeeView = GetEmployee();

            return View(emailDetailView);
        }

        #endregion

        #region Delete

        public ActionResult Delete(string id)
        {
            EmailDetailView emailDetailView = new EmailDetailView();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Email_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(emailDetailView);
            }
            #endregion

            emailDetailView.EmailView = this.GetEmailView(id);
            //emailDetailView.EmployeeView = GetEmployee();

            return View(emailDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            EmailDetailView emailDetailView = new EmailDetailView();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Email_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(emailDetailView);
            }
            #endregion

            emailDetailView.EmailView = this.GetEmailView(id);
            //emailDetailView.EmployeeView = GetEmployee();

            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._emailService.DeleteEmail(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(emailDetailView);
            }
        }

        #endregion

        #endregion

        #region SMTP settings

        #region Insert or Update Smtp Settings

        public JsonResult SmtpSettings_Insert(SmtpSettingsModel smtpSettings)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("SmtpSettings_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region if we want to Save into a file

            //try
            //{
            //    XmlSerializer writer = new XmlSerializer(typeof(SmtpSettingsModel));

            //    string path = Server.MapPath("~/config/xm.xml");
            //    StreamWriter file = new StreamWriter(path);
            //    writer.Serialize(file, smtpSettings);
            //    file.Close();

            //    hasCenter = true;
            //}
            //catch (Exception ex)
            //{
            //    hasCenter = false;
            //    return Json(ex.Message, JsonRequestBehavior.AllowGet);
            //}

            #endregion

            #region if we want to Save into Web.config

            try
            {
                Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration("~");
                MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

                if (mailSettings != null)
                {
                    mailSettings.Smtp.Network.EnableSsl = smtpSettings.EnableSsl;
                    mailSettings.Smtp.Network.Host = smtpSettings.Host;
                    mailSettings.Smtp.Network.Password = smtpSettings.Password;
                    mailSettings.Smtp.Network.Port = smtpSettings.Port;
                    mailSettings.Smtp.Network.UserName = smtpSettings.UserName;

                    configurationFile.Save();
                    
                }
            }
            catch (Exception ex)
            {
                //response.success = false;
                response.ErrorMessages.Add("SaveChangesProblemKey");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read Smtp Settings

        public JsonResult SmtpSettings_Read()
        {
            GetGeneralResponse<SmtpSettingsModel> smtpResponse = new GetGeneralResponse<SmtpSettingsModel>();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("SmtpSettings_Read");
            if (!hasPermission)
            {
                smtpResponse.ErrorMessages.Add("AccessDenied");
                return Json(smtpResponse, JsonRequestBehavior.AllowGet);
            }
            #endregion

            SmtpSettingsModel smtpSettings = new SmtpSettingsModel();


            #region to Read from a file

            //XmlSerializer reader = new XmlSerializer(typeof(SmtpSettingsModel));

            //try
            //{
            //    string path = Server.MapPath("~/config/xm.xml");
            //    StreamReader file = new StreamReader(path);

            //    smtp = (SmtpSettingsModel)reader.Deserialize(file);
            //    file.Close();
            //}
            //catch (Exception ex)
            //{
            //    return Json(ex.Message, JsonRequestBehavior.AllowGet);
            //}

            #endregion

            #region to read from Web.config

            try
            {
                Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration("~");
                MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

                if (mailSettings != null)
                {
                    smtpSettings.EnableSsl = mailSettings.Smtp.Network.EnableSsl;
                    smtpSettings.Host = mailSettings.Smtp.Network.Host;
                    smtpSettings.Password = mailSettings.Smtp.Network.Password;
                    smtpSettings.Port = mailSettings.Smtp.Network.Port;
                    smtpSettings.UserName = mailSettings.Smtp.Network.UserName;
                }
            }
            catch (Exception ex)
            {
                return Json(smtpResponse, JsonRequestBehavior.AllowGet);
            }

            #endregion

            smtpResponse.data = smtpSettings;

            return Json(smtpResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Emails_Read

        public JsonResult Emails_Read(Guid customerID, int pageSize, int pageNumber,string sort)
        {
            GetEmailsResponse response = new GetEmailsResponse();
            
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Emails_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            
            AjaxGetRequest request = new AjaxGetRequest()
            {
                ID = customerID,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            if (customerID != null)
            {
                response = _emailService.GetEmails(request,ConvertJsonToObject(sort));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Members

        private EmailView GetEmailView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetEmailResponse response = this._emailService.GetEmail(request);

            return response.EmailView;
        }
        #endregion

    }
}
