#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.FiscalCatalog;
using System.Web.Mvc;
using Services.ViewModels.Fiscals;
using Services.Messaging;
using Services.Messaging.FiscalCatalogService;
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class MoneyAccountEmployeeController: BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly IMoneyAccountEmployeeService _moneyAccountEmployeeService;
        #endregion

        #region Ctor

        public MoneyAccountEmployeeController(IEmployeeService employeeService, IMoneyAccountEmployeeService moneyAccountEmployeeService)
            : base(employeeService)
        {
            this._moneyAccountEmployeeService = moneyAccountEmployeeService;
            this._employeeService = employeeService;
        }
        #endregion

        #region Old Methods
        #region Index

        public ActionResult Index()
        {
            MoneyAccountEmployeeHomePageView moneyAccountEmployeeHomePageView = new MoneyAccountEmployeeHomePageView();
            moneyAccountEmployeeHomePageView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccountEmployee_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountEmployeeHomePageView);
            }
            #endregion
            
            
            moneyAccountEmployeeHomePageView.MoneyAccountEmployeeViews = this._moneyAccountEmployeeService.GetMoneyAccountEmployees().MoneyAccountEmployeeViews;

            return View(moneyAccountEmployeeHomePageView);
        }
        #endregion

        #region Create

        public ActionResult Create()
        {
            MoneyAccountEmployeeDetailView moneyAccountEmployeeDetailView = new MoneyAccountEmployeeDetailView();
            moneyAccountEmployeeDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccountEmployee_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountEmployeeDetailView);
            }
            #endregion

            return View(moneyAccountEmployeeDetailView);
        }

        [HttpPost]
        public ActionResult Create(MoneyAccountEmployeeDetailView moneyAccountEmployeeDetailView)
        {
            moneyAccountEmployeeDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccountEmployee_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountEmployeeDetailView);
            }
            #endregion

            
            if (ModelState.IsValid)
                try
                {
                    AddMoneyAccountEmployeeRequestOld request = new AddMoneyAccountEmployeeRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.EmployeeID = moneyAccountEmployeeDetailView.MoneyAccountEmployeeView.EmployeeID;
                    request.MoneyAccountID = moneyAccountEmployeeDetailView.MoneyAccountEmployeeView.MoneyAccountID;

                    GeneralResponse response = this._moneyAccountEmployeeService.AddMoneyAccountEmployee(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(moneyAccountEmployeeDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(moneyAccountEmployeeDetailView);
                }

            return View(moneyAccountEmployeeDetailView);
        }
        #endregion

        #region Edit

        public ActionResult Edit(string id)
        {
            MoneyAccountEmployeeDetailView moneyAccountEmployeeDetailView = new MoneyAccountEmployeeDetailView();
            moneyAccountEmployeeDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccountEmployee_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountEmployeeDetailView);
            }
            #endregion


            moneyAccountEmployeeDetailView.MoneyAccountEmployeeView = this.GetMoneyAccountEmployeeView(id);
            
            return View(moneyAccountEmployeeDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, MoneyAccountEmployeeDetailView moneyAccountEmployeeDetailView)
        {
            moneyAccountEmployeeDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccountEmployee_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountEmployeeDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditMoneyAccountEmployeeRequest request = new EditMoneyAccountEmployeeRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.EmployeeID = moneyAccountEmployeeDetailView.MoneyAccountEmployeeView.EmployeeID;
                    request.MoneyAccountID = moneyAccountEmployeeDetailView.MoneyAccountEmployeeView.MoneyAccountID;
                    request.RowVersion = moneyAccountEmployeeDetailView.MoneyAccountEmployeeView.RowVersion;

                    GeneralResponse response = this._moneyAccountEmployeeService.EditMoneyAccountEmployee(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(moneyAccountEmployeeDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(moneyAccountEmployeeDetailView);
                }

            return View(moneyAccountEmployeeDetailView);
        }
        #endregion

        #region Details

        public ActionResult Details(string id)
        {
            MoneyAccountEmployeeDetailView moneyAccountEmployeeDetailView = new MoneyAccountEmployeeDetailView();
            moneyAccountEmployeeDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccountEmployee_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountEmployeeDetailView);
            }
            #endregion


            MoneyAccountEmployeeView moneyAccountEmployeeView = this.GetMoneyAccountEmployeeView(id);

            moneyAccountEmployeeDetailView.MoneyAccountEmployeeView = moneyAccountEmployeeView;
            
            return View(moneyAccountEmployeeDetailView);
        }
        #endregion

        #region Delete

        public ActionResult Delete(string id)
        {
            MoneyAccountEmployeeDetailView moneyAccountEmployeeDetailView = new MoneyAccountEmployeeDetailView();
            moneyAccountEmployeeDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccountEmployee_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountEmployeeDetailView);
            }
            #endregion

            moneyAccountEmployeeDetailView.MoneyAccountEmployeeView = this.GetMoneyAccountEmployeeView(id);
            
            return View(moneyAccountEmployeeDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            MoneyAccountEmployeeDetailView moneyAccountEmployeeDetailView = new MoneyAccountEmployeeDetailView();
            moneyAccountEmployeeDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccountEmployee_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountEmployeeDetailView);
            }
            #endregion

            moneyAccountEmployeeDetailView.MoneyAccountEmployeeView = this.GetMoneyAccountEmployeeView(id);
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._moneyAccountEmployeeService.DeleteMoneyAccountEmployee(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(moneyAccountEmployeeDetailView);
            }
        }
        #endregion

        #endregion

        #region New Methods

        #region Read
        // 1392/11/06 تست شد
        public JsonResult MoneyAccountEmployees_Read(Guid MoneyAccountID, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<MoneyAccountEmployeeView>> response = new GetGeneralResponse<IEnumerable<MoneyAccountEmployeeView>>();

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _moneyAccountEmployeeService.GetMoneyAccountEmployees(MoneyAccountID, PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Insert
        // 1392/11/06 تست شد
        public JsonResult MoneyAccountEmployee_Insert(AddMoneyAccountEmployeeRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _moneyAccountEmployeeService.AddMoneyAccountEmployee(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete
        // 1392/11/06 تست شد
        public JsonResult MoneyAccountEmployees_Delete(IEnumerable<DeleteMoneyAccountEmployeeRequest> requests,Guid MoneyAccountID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _moneyAccountEmployeeService.DeleteMoneyAccountEmployee(requests, MoneyAccountID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #endregion

        #region Private Members

        private MoneyAccountEmployeeView GetMoneyAccountEmployeeView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetMoneyAccountEmployeeResponse response = this._moneyAccountEmployeeService.GetMoneyAccountEmployee(request);

            return response.MoneyAccountEmployeeView;
        }

        #endregion

    }
}
