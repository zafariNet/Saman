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
using Controllers.ViewModels;
using Services.ViewModels.Employees;
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class MoneyAccountController: BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly IMoneyAccountService _moneyAccountService;

        private readonly IMoneyAccountEmployeeService _moneyAccountEmployeeService;
        #endregion

        #region Ctor

        public MoneyAccountController(IEmployeeService employeeService, IMoneyAccountService moneyAccountService
            , IMoneyAccountEmployeeService moneyAccountEmployeeService)
            : base(employeeService)
        {
            this._moneyAccountService = moneyAccountService;
            this._employeeService = employeeService;
            _moneyAccountEmployeeService = moneyAccountEmployeeService;
        }
        #endregion

        #region Index

        public ActionResult Index()
        {
            MoneyAccountHomePageView moneyAccountHomePageView = new MoneyAccountHomePageView();
            moneyAccountHomePageView.EmployeeView = GetEmployee();

            //#region Check Access
            //bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Read");
            //if (!hasPermission)
            //{
            //    ModelState.AddModelError("", "AccessDenied");
            //    return View(moneyAccountHomePageView);
            //}
            //#endregion
            
            moneyAccountHomePageView.MoneyAccountViews = this._moneyAccountService.GetMoneyAccounts().MoneyAccountViews;

            return View(moneyAccountHomePageView);
        }
        #endregion

        #region Create

        public ActionResult Create()
        {
            MoneyAccountDetailView moneyAccountDetailView = new MoneyAccountDetailView();
            moneyAccountDetailView.EmployeeView = GetEmployee();

            #region Check Access

            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountDetailView);
            }
            #endregion

            return View(moneyAccountDetailView);
        }

        [HttpPost]
        public ActionResult Create(MoneyAccountDetailView moneyAccountDetailView)
        {
            moneyAccountDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountDetailView);
            }
            #endregion

            
            if (ModelState.IsValid)
                try
                {
                    AddMoneyAccountRequestOld request = new AddMoneyAccountRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.AccountName = moneyAccountDetailView.MoneyAccountView.AccountName;
                    request.BAccountInfo = moneyAccountDetailView.MoneyAccountView.BAccountInfo;
                    request.BAccountNumber = moneyAccountDetailView.MoneyAccountView.BAccountNumber;
                    request.IsBankAccount = moneyAccountDetailView.MoneyAccountView.IsBankAccount;
                    request.Pay = moneyAccountDetailView.MoneyAccountView.Pay;
                    request.Receipt = moneyAccountDetailView.MoneyAccountView.Receipt;

                    GeneralResponse response = this._moneyAccountService.AddMoneyAccount(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(moneyAccountDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(moneyAccountDetailView);
                }

            return View(moneyAccountDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            MoneyAccountDetailView moneyAccountDetailView = new MoneyAccountDetailView();
            moneyAccountDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountDetailView);
            }
            #endregion

            moneyAccountDetailView.MoneyAccountView = this.GetMoneyAccountView(id);
            

            return View(moneyAccountDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, MoneyAccountDetailView moneyAccountDetailView)
        {
            moneyAccountDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountDetailView);
            }
            #endregion

            
            if (ModelState.IsValid)
                try
                {
                    EditMoneyAccountRequestOld request = new EditMoneyAccountRequestOld();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.AccountName = moneyAccountDetailView.MoneyAccountView.AccountName;
                    request.BAccountInfo = moneyAccountDetailView.MoneyAccountView.BAccountInfo;
                    request.BAccountNumber = moneyAccountDetailView.MoneyAccountView.BAccountNumber;
                    request.IsBankAccount = moneyAccountDetailView.MoneyAccountView.IsBankAccount;
                    request.Pay = moneyAccountDetailView.MoneyAccountView.Pay;
                    request.Receipt = moneyAccountDetailView.MoneyAccountView.Receipt;
                    request.RowVersion = moneyAccountDetailView.MoneyAccountView.RowVersion;

                    GeneralResponse response = this._moneyAccountService.EditMoneyAccount(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(moneyAccountDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(moneyAccountDetailView);
                }

            return View(moneyAccountDetailView);
        }
        #endregion

        #region Details

        public ActionResult Details(string id)
        {
            MoneyAccountDetailView moneyAccountDetailView = new MoneyAccountDetailView();
            moneyAccountDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountDetailView);
            }
            #endregion

            MoneyAccountView moneyAccountView = this.GetMoneyAccountView(id);

            moneyAccountDetailView.MoneyAccountView = moneyAccountView;
            
            return View(moneyAccountDetailView);
        }
        #endregion

        #region Delete

        public ActionResult Delete(string id)
        {
            MoneyAccountDetailView moneyAccountDetailView = new MoneyAccountDetailView();
            moneyAccountDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountDetailView);
            }
            #endregion
   
            moneyAccountDetailView.MoneyAccountView = this.GetMoneyAccountView(id);
            

            return View(moneyAccountDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            MoneyAccountDetailView moneyAccountDetailView = new MoneyAccountDetailView();
            moneyAccountDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountDetailView);
            }
            #endregion

            moneyAccountDetailView.MoneyAccountView = this.GetMoneyAccountView(id);
            

            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._moneyAccountService.DeleteMoneyAccount(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(moneyAccountDetailView);
            }
        }
        #endregion


        #region Permitted Employees

        public ActionResult MoneyAccountEmployee(string id)
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

            moneyAccountEmployeeDetailView.MoneyAccountView = GetMoneyAccountView(id);
            moneyAccountEmployeeDetailView.MoneyAccountEmployeeViews = _moneyAccountEmployeeService.GetMoneyAccountEmployees(Guid.Parse(id)).MoneyAccountEmployeeViews;

            #region DropDownList For Employees

            moneyAccountEmployeeDetailView.EmployeeViews = _employeeService.GetEmployees().EmployeeViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (moneyAccountEmployeeDetailView.EmployeeViews.Count() > 0)
                foreach (EmployeeView employeeView in moneyAccountEmployeeDetailView.EmployeeViews)
                {
                    list.Add(new DropDownItem { Value = employeeView.ID.ToString(), Text = employeeView.Name });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["Employees"] = selectList;

            #endregion

            return View(moneyAccountEmployeeDetailView);

        }

        // Create
        [HttpPost]
        public ActionResult MoneyAccountEmployee(string id, MoneyAccountEmployeeDetailView moneyAccountEmployeeDetailView)
        {
            moneyAccountEmployeeDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccountEmployee_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(moneyAccountEmployeeDetailView);
            }
            #endregion

            moneyAccountEmployeeDetailView.MoneyAccountEmployeeViews = _moneyAccountEmployeeService.GetMoneyAccountEmployees(Guid.Parse(id)).MoneyAccountEmployeeViews;
            moneyAccountEmployeeDetailView.MoneyAccountView = _moneyAccountService.GetMoneyAccount(new GetRequest() { ID = Guid.Parse(id) }).MoneyAccountView;

            #region DropDownList For Employees

            moneyAccountEmployeeDetailView.EmployeeViews = _employeeService.GetEmployees().EmployeeViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (moneyAccountEmployeeDetailView.EmployeeViews.Count() > 0)
                foreach (EmployeeView employeeView in moneyAccountEmployeeDetailView.EmployeeViews)
                {
                    list.Add(new DropDownItem { Value = employeeView.ID.ToString(), Text = employeeView.Name });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["Employees"] = selectList;

            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddMoneyAccountEmployeeRequestOld request = new AddMoneyAccountEmployeeRequestOld();

                    request.MoneyAccountID = Guid.Parse(id);
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.EmployeeID = moneyAccountEmployeeDetailView.EmployeeViewForInsert.ID;
                    
                    GeneralResponse response = _moneyAccountEmployeeService.AddMoneyAccountEmployee(request);

                    if (!response.success)
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

            // Reload content of grid:
            moneyAccountEmployeeDetailView.MoneyAccountEmployeeViews = _moneyAccountEmployeeService.GetMoneyAccountEmployees(Guid.Parse(id)).MoneyAccountEmployeeViews;

            return View(moneyAccountEmployeeDetailView);
        }

        // Delete
        [HttpPost]
        public ActionResult MoneyAccountEmployee_Delete(MoneyAccountEmployeeView moneyAccountEmployeeView)
        {
            MoneyAccountEmployeeDetailView moneyAccountEmployeeDetailView = new MoneyAccountEmployeeDetailView();
            GeneralResponse response=new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccountEmployee_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return View(moneyAccountEmployeeDetailView);
            }
            #endregion


            response = this._moneyAccountEmployeeService.DeleteMoneyAccountEmployee(moneyAccountEmployeeView.MoneyAccountID, moneyAccountEmployeeView.EmployeeID);

            if (!response.success)
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
            }
            return View(moneyAccountEmployeeDetailView);
        }

        #endregion
        
        #region New Methods

        #region Read
        
        public JsonResult MoneyAccounts_Read( int? pageSize, int? pageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<MoneyAccountView>> response = new GetGeneralResponse<IEnumerable<MoneyAccountView>>();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _moneyAccountService.GetAllMoneyAccounts(PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MoneyAccounts_Read_NoPermission(int? pageSize, int? pageNumber, string sort)
        {
            GetGeneralResponse<IEnumerable<MoneyAccountView>> response = new GetGeneralResponse<IEnumerable<MoneyAccountView>>();


            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _moneyAccountService.GetAllMoneyAccounts(PageSize, PageNumber, ConvertJsonToObject(sort));
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult MoneyAccouns_Insert(IEnumerable<AddMoneyAccountRequest> requests)
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

            response = _moneyAccountService.AddMoneyAccounts(requests,GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult MoneyAccount_Update(IEnumerable<EditMoneyAccountRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access

            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _moneyAccountService.EditMoneyAccounts(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult MoneyAccount_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access

            bool hasPermission = GetEmployee().IsGuaranteed("MoneyAccount_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _moneyAccountService.DeleteMoneyAccounts(requests);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Moveing

        public JsonResult MoneyAccount_MoveUp(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _moneyAccountService.MoveUp(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MoneyAccount_MoveDown(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _moneyAccountService.MoveDown(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Private Members

        private MoneyAccountView GetMoneyAccountView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetMoneyAccountResponse response = this._moneyAccountService.GetMoneyAccount(request);

            return response.MoneyAccountView;
        }

        #endregion

    }
}
