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

namespace Controllers.Controllers
{
    [Authorize]
    public class SpecialNumberController: BaseController
    {

        #region Declar

        private readonly IEmployeeService _employeeService;

        private readonly ISpecialNumberService _specialNumberService;

        #endregion

        #region Ctor

        public SpecialNumberController(IEmployeeService employeeService, ISpecialNumberService specialNumberService)
            : base(employeeService)
        {
            this._specialNumberService = specialNumberService;
            this._employeeService = employeeService;
        }

        #endregion
        #region Old Methods

        public ActionResult Index()
        {
            SpecialNumberHomePageView specialNumberHomePageView = new SpecialNumberHomePageView();
            specialNumberHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SpecialNumbers_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(specialNumberHomePageView);
            }
            #endregion

            specialNumberHomePageView.SpecialNumberViews = this._specialNumberService.GetSpecialNumbers().SpecialNumberViews;

            return View(specialNumberHomePageView);
        }

        public ActionResult Create()
        {
            SpecialNumberDetailView specialNumberDetailView = new SpecialNumberDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SpecialNumbers_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(specialNumberDetailView);
            }
            #endregion

            specialNumberDetailView.EmployeeView = GetEmployee();

            return View(specialNumberDetailView);
        }

        [HttpPost]
        public ActionResult Create(SpecialNumberDetailView specialNumberDetailView)
        {
            specialNumberDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SpecialNumbers_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(specialNumberDetailView);
            }
            #endregion
            
            if (ModelState.IsValid)
                try
                {
                    AddSpecialNumberRequestOld request = new AddSpecialNumberRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.FromNumber = specialNumberDetailView.SpecialNumberView.FromNumber;
                    request.ToNumber = specialNumberDetailView.SpecialNumberView.ToNumber;
                    request.Note = specialNumberDetailView.SpecialNumberView.Note;

                    GeneralResponse response = this._specialNumberService.AddSpecialNumber(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(specialNumberDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(specialNumberDetailView);
                }

            return View(specialNumberDetailView);
        }

        public ActionResult Edit(string id)
        {
            SpecialNumberDetailView specialNumberDetailView = new SpecialNumberDetailView();
            specialNumberDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SpecialNumbers_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(specialNumberDetailView);
            }
            #endregion

            specialNumberDetailView.SpecialNumberView = this.GetSpecialNumberView(id);
            
            return View(specialNumberDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, SpecialNumberDetailView specialNumberDetailView)
        {
            specialNumberDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SpecialNumbers_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(specialNumberDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditSpecialNumberRequestOld request = new EditSpecialNumberRequestOld();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.FromNumber = specialNumberDetailView.SpecialNumberView.FromNumber;
                    request.ToNumber = specialNumberDetailView.SpecialNumberView.ToNumber;
                    request.Note = specialNumberDetailView.SpecialNumberView.Note;
                    request.RowVersion = specialNumberDetailView.SpecialNumberView.RowVersion;

                    GeneralResponse response = this._specialNumberService.EditSpecialNumber(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(specialNumberDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(specialNumberDetailView);
                }

            return View(specialNumberDetailView);
        }

        public ActionResult Details(string id)
        {
            SpecialNumberDetailView specialNumberDetailView = new SpecialNumberDetailView();
            specialNumberDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SpecialNumbers_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(specialNumberDetailView);
            }
            #endregion

            SpecialNumberView specialNumberView = this.GetSpecialNumberView(id);

            specialNumberDetailView.SpecialNumberView = specialNumberView;
            
            return View(specialNumberDetailView);
        }

        public ActionResult Delete(string id)
        {
            SpecialNumberDetailView specialNumberDetailView = new SpecialNumberDetailView();
            specialNumberDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SpecialNumbers_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(specialNumberDetailView);
            }
            #endregion

            specialNumberDetailView.SpecialNumberView = this.GetSpecialNumberView(id);
            
            return View(specialNumberDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            SpecialNumberDetailView specialNumberDetailView = new SpecialNumberDetailView();
            specialNumberDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SpecialNumbers_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(specialNumberDetailView);
            }
            #endregion

            specialNumberDetailView.SpecialNumberView = this.GetSpecialNumberView(id);
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._specialNumberService.DeleteSpecialNumber(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(specialNumberDetailView);
            }
        }

        #endregion

        #region New methods

        #region Read

        public JsonResult SpecialNumbers_Read(int? pageSize, int? pageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<SpecialNumberView>> response = new GetGeneralResponse<IEnumerable<SpecialNumberView>>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("SpecialNumbers_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _specialNumberService.GetSpecialNumbers(PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Insert

        public JsonResult SpecialNumbers_Insert(IEnumerable<AddSpecialNumberRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("SpecialNumbers_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _specialNumberService.AddSpecialNumbers(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region Update

        public JsonResult SpecialNumbers_Update(IEnumerable<EditSpecialNumberRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("SpecialNumbers_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _specialNumberService.EditSpecialNumbers(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Delete

        public JsonResult SpecialNumbers_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("SpecialNumbers_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _specialNumberService.DeleteSpecialNumbers(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion
        #region Private Members

        private SpecialNumberView GetSpecialNumberView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetSpecialNumberResponse response = this._specialNumberService.GetSpecialNumber(request);

            return response.SpecialNumberView;
        }

        #endregion

    }
}
