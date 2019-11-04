#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.CustomerCatalog;
using System.Web.Mvc;
//using System.Web.UI.WebControls;
using Services.ViewModels.Customers;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;

#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class SmsController: BaseController
    {
        #region Declares
        
        private readonly IEmployeeService _employeeService;

        private readonly ISmsService _smsService;
        
        #endregion

        #region Ctor
        
        public SmsController(IEmployeeService employeeService, ISmsService smsService)
            : base(employeeService)
        {
            this._smsService = smsService;
            this._employeeService = employeeService;
        }
        
        #endregion

        #region Old Methods
        
        //public ActionResult Index()
        //{
        //    SmsHomePageView smsHomePageView = new SmsHomePageView();
        //    smsHomePageView.EmployeeView = GetEmployee();
        //    smsHomePageView.SmsViews = this._smsService.GetSmss().SmsViews;

        //    return View(smsHomePageView);
        //}

        //public ActionResult Create()
        //{
        //    SmsDetailView smsDetailView = new SmsDetailView();
        //    smsDetailView.EmployeeView = GetEmployee();

        //    return View(smsDetailView);
        //}

        //[HttpPost]
        //public ActionResult Create(SmsDetailView smsDetailView)
        //{
        //    if (ModelState.IsValid)
        //        try
        //        {
        //            AddSmsRequest request = new AddSmsRequest();
        //            request.CreateEmployeeID = GetEmployee().ID;
        //            request.Body = smsDetailView.SmsView.Body;
        //            request.CustomerID = smsDetailView.SmsView.CustomerID;
        //            request.Note = smsDetailView.SmsView.Note;
        //            request.Sent = smsDetailView.SmsView.Sent;

        //            GeneralResponse response = this._smsService.AddSms(request);

        //            if (response.success)
        //                return RedirectToAction("Index");
        //            else
        //            {
        //                foreach (string error in response.ErrorMessages)
        //                    ModelState.AddModelError("", error);
        //                return View(smsDetailView);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", ex.Message);
        //            return View(smsDetailView);
        //        }

        //    return View(smsDetailView);
        //}

        //public ActionResult Edit(string id)
        //{
        //    SmsDetailView smsDetailView = new SmsDetailView();
        //    smsDetailView.SmsView = this.GetSmsView(id);
        //    //smsDetailView.EmployeeView = GetEmployee();

        //    return View(smsDetailView);
        //}

        //[HttpPost]
        //public ActionResult Edit(string id, SmsDetailView smsDetailView)
        //{
        //    if (ModelState.IsValid)
        //        try
        //        {
        //            EditSmsRequest request = new EditSmsRequest();

        //            request.ID = Guid.Parse(id);
        //            request.ModifiedEmployeeID = GetEmployee().ID;
        //            request.Body = smsDetailView.SmsView.Body;
        //            request.CustomerID = smsDetailView.SmsView.CustomerID;
        //            request.Note = smsDetailView.SmsView.Note;
        //            request.Sent = smsDetailView.SmsView.Sent;
        //            request.RowVersion = smsDetailView.SmsView.RowVersion;

        //            GeneralResponse response = this._smsService.EditSms(request);

        //            if (response.success)
        //                return RedirectToAction("Index");
        //            else
        //            {
        //                foreach (string error in response.ErrorMessages)
        //                    ModelState.AddModelError("", error);
        //                return View(smsDetailView);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", ex.Message);
        //            return View(smsDetailView);
        //        }

        //    return View(smsDetailView);
        //}

        //public ActionResult Details(string id)
        //{
        //    SmsView smsView = this.GetSmsView(id);

        //    SmsDetailView smsDetailView = new SmsDetailView();
        //    smsDetailView.SmsView = smsView;
        //    // smsDetailView.EmployeeView = GetEmployee();

        //    return View(smsDetailView);
        //}

        //public ActionResult Delete(string id)
        //{
        //    SmsDetailView smsDetailView = new SmsDetailView();
        //    smsDetailView.SmsView = this.GetSmsView(id);
        //    //smsDetailView.EmployeeView = GetEmployee();

        //    return View(smsDetailView);
        //}

        //[HttpPost]
        //public ActionResult Delete(string id, FormCollection collection)
        //{
        //    SmsDetailView smsDetailView = new SmsDetailView();
        //    smsDetailView.SmsView = this.GetSmsView(id);
        //    //smsDetailView.EmployeeView = GetEmployee();
        //    
        //    DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

        //    GeneralResponse response = this._smsService.DeleteSms(request);

        //    if (response.success)
        //        return RedirectToAction("Index");
        //    else
        //    {
        //        foreach (string error in response.ErrorMessages)
        //            ModelState.AddModelError("", error);
        //        return View(smsDetailView);
        //    }
        //}
        
        #endregion

        #region Json

        public JsonResult Smss_Read(Guid CustomerID, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<SmsView>> response=new GetGeneralResponse<IEnumerable<SmsView>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Smss_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

             response = _smsService.GetSmss(CustomerID, PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult Sms_Delete(Guid SmsID)
        //{
        //    GeneralResponse response = new GeneralResponse();

        //    response = _smsService.DeleteSms(new DeleteRequest() { ID = SmsID });

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult Sms_Insert(Guid CustomerID, string Body, string Note)
        //{
        //    AddSmsRequest request = new AddSmsRequest();
        //    request.CreateEmployeeID = GetEmployee().ID;
        //    request.Body = Body;
        //    request.Note = Note;
        //    request.CustomerID = CustomerID;

        //    GeneralResponse response = this._smsService.AddSms(request);

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult Sms_Update(Guid smsID, string smsTitle, string smsDescription, short priority, short state, int rowVersion)
        //{
        //    EditSmsRequest request = new EditSmsRequest();
        //    request.ID = smsID;
        //    request.RowVersion = rowVersion;
        //    request.ModifiedEmployeeID = GetEmployee().ID;
        //    //request.SmsTitle = smsTitle;
        //    //request.SmsDescription = smsDescription;
        //    //request.Priority = priority;
        //    //request.State = state;

        //    GeneralResponse response = this._smsService.EditSms(request);

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Private Members

        private SmsView GetSmsView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetSmsResponse response = this._smsService.GetSms(request);

            return response.SmsView;
        }

        #endregion

    }
}
