#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.Interfaces;
using Controllers.ViewModels.FiscalCatalog;
using System.Web.Mvc;
using Services.ViewModels.Fiscals;
using Services.Messaging;
using Services.Messaging.FiscalCatalogService;
using Kendo.Mvc.UI;
using Services.ViewModels.Employees;
using Controllers.ViewModels;
using Model.Fiscals;
using Controllers.ViewModels.Reports;
using Services.ViewModels.Customers;
using Infrastructure.Querying;
using Services.Messaging.CustomerCatalogService;

#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class FiscalController: BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly IFiscalService _fiscalService;

        private readonly IMoneyAccountService _moneyAccountService;

        private readonly ICustomerService _customerService;

        private readonly ILevelTypeService _levelTypeService;

        private readonly ICustomerLevelService _customerLevelService;

        #endregion

        #region Ctor

        public FiscalController(IEmployeeService employeeService, IFiscalService fiscalService
            , IMoneyAccountService moneyAccountService, ICustomerService customerService
            , ILevelTypeService levelTypeService, ICustomerLevelService customerLevelService)
            : base(employeeService, customerService)
        {
            this._fiscalService = fiscalService;
            this._employeeService = employeeService;
            _moneyAccountService = moneyAccountService;
            _customerService = customerService;
            _levelTypeService = levelTypeService;
            _customerLevelService = customerLevelService;
        }

        #endregion

        #region Ajax Read

        public ActionResult Fiscal_Read([DataSourceRequest] DataSourceRequest request)
        {
            GetFiscalsResponse  fiscalResponse=new GetFiscalsResponse();
            FiscalHomePageView fiscalHomePageView = new FiscalHomePageView();
            fiscalHomePageView.EmployeeView = GetEmployee();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Read");
            if (!hasPermission)
            {
                var _result = new DataSourceResult()
                {
                    Data = fiscalResponse.FiscalViews,
                    Total = fiscalResponse.Count
                };
       
                ModelState.AddModelError("", "AccessDenied");
                return Json(_result);
            }

            #endregion

            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            getRequest.PageNumber = request.Page;
            Guid employeeID = GetEmployee().ID;
            fiscalResponse = _fiscalService.GetFiscals(getRequest, employeeID);

            #region Filling CanConfirm

            IList<FiscalView> fiscalViews = new List<FiscalView>();
            foreach (FiscalView fiscalView in fiscalResponse.FiscalViews)
            {
                //fiscalView.CanConfirm = fiscalView.EmployeesWhoCanConfirm.Contains(fiscalHomePageView.EmployeeView)
                //    && (fiscalView.ConfirmInt != 2) && (fiscalView.ConfirmInt != 3);
                //fiscalViews.Add(fiscalView);
            }

            #endregion

            fiscalHomePageView.FiscalViews = fiscalViews;
            fiscalHomePageView.Count = fiscalResponse.Count;

            var result = new DataSourceResult()
            {
                Data = fiscalResponse.FiscalViews,
                Total = fiscalResponse.Count
            };
            return Json(result);
        }

        public ActionResult Fiscal_AllRead([DataSourceRequest] DataSourceRequest request)
        {
            FiscalHomePageView fiscalHomePageView = new FiscalHomePageView();
            fiscalHomePageView.EmployeeView = GetEmployee();
            GetFiscalsResponse fiscalResponse=new GetFiscalsResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Read");
            if (!hasPermission)
            {
                var _result = new DataSourceResult()
                {
                    Data = fiscalResponse.FiscalViews,
                    Total = fiscalResponse.Count
                };
                ModelState.AddModelError("", "AccessDenied"); ;
                return Json(_result);
            }

            #endregion


            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            getRequest.PageNumber = request.Page;

            fiscalResponse = _fiscalService.GetFiscals(getRequest);
            
            #region Filling CanConfirm

            IList<FiscalView> fiscalViews = new List<FiscalView>();
            foreach (FiscalView fiscalView in fiscalResponse.FiscalViews)
            {
                //fiscalView.CanConfirm = fiscalView.EmployeesWhoCanConfirm.Contains(fiscalHomePageView.EmployeeView)
                //    && (fiscalView.ConfirmInt != 2) && (fiscalView.ConfirmInt != 3);
                //fiscalViews.Add(fiscalView);
            }

            #endregion

            fiscalHomePageView.FiscalViews = fiscalViews;
            fiscalHomePageView.Count = fiscalResponse.Count;

            var result = new DataSourceResult()
            {
                Data = fiscalResponse.FiscalViews,
                Total = fiscalResponse.Count
            };
            return Json(result);
        }

        public ActionResult Fiscal_WithMeRead([DataSourceRequest] DataSourceRequest request)
        {
            FiscalHomePageView fiscalHomePageView = new FiscalHomePageView();
            fiscalHomePageView.EmployeeView = GetEmployee();
            GetFiscalsResponse  fiscalResponse=new GetFiscalsResponse();
            
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Read");
            if (!hasPermission)
            {
                var _result = new DataSourceResult()
                {
                    Data = fiscalResponse.FiscalViews,
                    Total = fiscalResponse.Count
                };
                ModelState.AddModelError("", "AccessDenied"); ;
                return Json(_result);
            }

            #endregion

            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            getRequest.PageNumber = request.Page;
            string employeeID = GetEmployee().ID.ToString();
            fiscalResponse = _fiscalService.GetFiscalsCreatedOrConfirmedWithMe(getRequest, employeeID);

            #region Filling CanConfirm

            IList<FiscalView> fiscalViews = new List<FiscalView>();
            foreach (FiscalView fiscalView in fiscalResponse.FiscalViews)
            {
                //fiscalView.CanConfirm = fiscalView.EmployeesWhoCanConfirm.Contains(fiscalHomePageView.EmployeeView)
                //    && (fiscalView.ConfirmInt != 2) && (fiscalView.ConfirmInt != 3);
                //fiscalViews.Add(fiscalView);
            }

            #endregion

            fiscalHomePageView.FiscalViews = fiscalViews;
            fiscalHomePageView.Count = fiscalResponse.Count;

            var result = new DataSourceResult()
            {
                Data = fiscalResponse.FiscalViews,
                Total = fiscalResponse.Count
            };
            return Json(result);
        }

        public ActionResult Fiscal_Confirm([DataSourceRequest] DataSourceRequest request)
        {
            FiscalHomePageView fiscalHomePageView = new FiscalHomePageView();
            fiscalHomePageView.EmployeeView = GetEmployee();
            GetFiscalsResponse fiscalResponse=new GetFiscalsResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_DoConfirm");
            if (!hasPermission)
            {
                var _result = new DataSourceResult()
                {
                    Data = fiscalResponse.FiscalViews,
                    Total = fiscalResponse.Count
                };
                ModelState.AddModelError("", "AccessDenied"); ;
                return Json(_result);
            }

            #endregion


            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            getRequest.PageNumber = request.Page;
            string employeeID = GetEmployee().ID.ToString();
            fiscalResponse = _fiscalService.GetFiscalsCanConfirm(getRequest, employeeID);

            #region Filling CanConfirm

            IList<FiscalView> fiscalViews = new List<FiscalView>();
            foreach (FiscalView fiscalView in fiscalResponse.FiscalViews)
            {
                //fiscalView.CanConfirm = fiscalView.EmployeesWhoCanConfirm.Contains(fiscalHomePageView.EmployeeView)
                //    && (fiscalView.ConfirmInt != 2) && (fiscalView.ConfirmInt != 3);
                //fiscalViews.Add(fiscalView);
            }

            #endregion

            fiscalHomePageView.FiscalViews = fiscalViews;
            fiscalHomePageView.Count = fiscalResponse.Count;

            var result = new DataSourceResult()
            {
                Data = fiscalResponse.FiscalViews,
                Total = fiscalResponse.Count
            };
            return Json(result);
        }

        public ActionResult Fiscal_Customer(string id, [DataSourceRequest] DataSourceRequest request)
        {
            FiscalHomePageView fiscalHomePageView = new FiscalHomePageView();
            fiscalHomePageView.EmployeeView = GetEmployee();
            GetFiscalsResponse fiscalResponse=new GetFiscalsResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Read");
            if (!hasPermission)
            {
                var _result = new DataSourceResult()
                {
                    Data = fiscalResponse.FiscalViews,
                    Total = fiscalResponse.Count
                };
                ModelState.AddModelError("", "AccessDenied"); ;
                return Json(_result);
            }

            #endregion

            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            getRequest.PageNumber = request.Page;

            Guid customerID = Guid.Parse(id);
            fiscalResponse = _fiscalService.GetFiscalsOfCustomer(getRequest, customerID);

            #region Filling CanConfirm

            IList<FiscalView> fiscalViews = new List<FiscalView>();
            foreach (FiscalView fiscalView in fiscalResponse.FiscalViews)
            {
                //fiscalView.CanConfirm = fiscalView.EmployeesWhoCanConfirm.Contains(fiscalHomePageView.EmployeeView)
                //    && (fiscalView.ConfirmInt != 2) && (fiscalView.ConfirmInt != 3);
                //fiscalViews.Add(fiscalView);
            }

            #endregion

            fiscalHomePageView.FiscalViews = fiscalViews;
            fiscalHomePageView.Count = fiscalResponse.Count;

            var result = new DataSourceResult()
            {
                Data = fiscalResponse.FiscalViews,
                Total = fiscalResponse.Count
            };
            return Json(result);
        }

        #endregion

        #region Old Methods

        #region Index

        public ActionResult Index()
        {
            FiscalHomePageView fiscalHomePageView = new FiscalHomePageView();
            fiscalHomePageView.EmployeeView = GetEmployee();
            
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(fiscalHomePageView);
            }
            #endregion

            GetFiscalsResponse fiscalResponse = new GetFiscalsResponse();
            AjaxGetRequest getRequest = new AjaxGetRequest();
            getRequest.PageSize = 10;
            getRequest.PageNumber = 1;
            Guid employeeID = GetEmployee().ID;
            fiscalResponse = _fiscalService.GetFiscals(getRequest, employeeID);
            #region Filling CanConfirm

            IList<FiscalView> fiscalViews = new List<FiscalView>();
            foreach (FiscalView fiscalView in fiscalResponse.FiscalViews)
            {
                //fiscalView.CanConfirm = fiscalView.EmployeesWhoCanConfirm.Contains(fiscalHomePageView.EmployeeView)
                //    && (fiscalView.ConfirmInt != 2) && (fiscalView.ConfirmInt != 3);
                //fiscalViews.Add(fiscalView);
            }

            #endregion

            fiscalHomePageView.FiscalViews = fiscalViews;

            return View(fiscalHomePageView);
        }

        public ActionResult WithMe()
        {
            FiscalHomePageView fiscalHomePageView = new FiscalHomePageView();
            fiscalHomePageView.EmployeeView = GetEmployee();
            
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_WithMeRead");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(fiscalHomePageView);
            }
            #endregion

            GetFiscalsResponse fiscalResponse = new GetFiscalsResponse();
            AjaxGetRequest getRequest = new AjaxGetRequest();
            getRequest.PageSize = 10;
            getRequest.PageNumber = 1;
            string employeeID = GetEmployee().ID.ToString();
            fiscalResponse = _fiscalService.GetFiscalsCreatedOrConfirmedWithMe(getRequest, employeeID);

            #region Filling CanConfirm

            IList<FiscalView> fiscalViews = new List<FiscalView>();
            foreach (FiscalView fiscalView in fiscalResponse.FiscalViews)
            {
                //fiscalView.CanConfirm = fiscalView.EmployeesWhoCanConfirm.Contains(fiscalHomePageView.EmployeeView)
                //    && (fiscalView.ConfirmInt != 2) && (fiscalView.ConfirmInt != 3);
                //fiscalViews.Add(fiscalView);
            }

            #endregion

            fiscalHomePageView.FiscalViews = fiscalViews;

            return View(fiscalHomePageView);
        }

        public ActionResult Confirm()
        {
            FiscalHomePageView fiscalHomePageView = new FiscalHomePageView();
            fiscalHomePageView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_DoConfirm");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(fiscalHomePageView);
            }
            #endregion

            GetFiscalsResponse fiscalResponse = new GetFiscalsResponse();
            AjaxGetRequest getRequest = new AjaxGetRequest();
            getRequest.PageSize = 10;
            getRequest.PageNumber = 1;
            string employeeID = GetEmployee().ID.ToString();
            fiscalResponse = _fiscalService.GetFiscalsCanConfirm(getRequest, employeeID);

            #region Filling CanConfirm

            IList<FiscalView> fiscalViews = new List<FiscalView>();
            foreach (FiscalView fiscalView in fiscalResponse.FiscalViews)
            {
                //fiscalView.CanConfirm = fiscalView.EmployeesWhoCanConfirm.Contains(fiscalHomePageView.EmployeeView)
                //    && (fiscalView.ConfirmInt != 2) && (fiscalView.ConfirmInt != 3);
                //fiscalViews.Add(fiscalView);
            }

            #endregion

            fiscalHomePageView.FiscalViews = fiscalViews;

            return View(fiscalHomePageView);
        }

        public ActionResult All()
        {
            FiscalHomePageView fiscalHomePageView = new FiscalHomePageView();
            fiscalHomePageView.EmployeeView = GetEmployee();
            
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(fiscalHomePageView);
            }
            #endregion


            GetFiscalsResponse fiscalResponse = new GetFiscalsResponse();
            AjaxGetRequest getRequest = new AjaxGetRequest();
            getRequest.PageSize = 10;
            getRequest.PageNumber = 1;

            fiscalResponse = _fiscalService.GetFiscals(getRequest);

            #region Filling CanConfirm

            IList<FiscalView> fiscalViews = new List<FiscalView>();
            foreach (FiscalView fiscalView in fiscalResponse.FiscalViews)
            {
                //fiscalView.CanConfirm = fiscalView.EmployeesWhoCanConfirm.Contains(fiscalHomePageView.EmployeeView)
                //    && (fiscalView.ConfirmInt != 2) && (fiscalView.ConfirmInt != 3);
                //fiscalViews.Add(fiscalView);
            }

            #endregion

            fiscalHomePageView.FiscalViews = fiscalViews;

            return View(fiscalHomePageView);
        }

        public ActionResult CustomerFiscal(string id)
        {
            FiscalHomePageView fiscalHomePageView = new FiscalHomePageView();
            fiscalHomePageView.EmployeeView = GetEmployee();
            
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(fiscalHomePageView);
            }
            #endregion

            GetFiscalsResponse fiscalResponse = new GetFiscalsResponse();
            fiscalHomePageView.CustomerView = GetCustomer(id);

            AjaxGetRequest getRequest = new AjaxGetRequest();
            getRequest.PageSize = 10;
            getRequest.PageNumber = 1;
            string employeeID = GetEmployee().ID.ToString();
            fiscalResponse = _fiscalService.GetFiscalsOfCustomer(getRequest, Guid.Parse(id));

            #region Filling CanConfirm

            IList<FiscalView> fiscalViews = new List<FiscalView>();
            foreach (FiscalView fiscalView in fiscalResponse.FiscalViews)
            {
                //fiscalView.CanConfirm = fiscalView.EmployeesWhoCanConfirm.Contains(fiscalHomePageView.EmployeeView)
                //    && (fiscalView.ConfirmInt != 2) && (fiscalView.ConfirmInt != 3);
                //fiscalViews.Add(fiscalView);
            }

            #endregion

            fiscalHomePageView.FiscalViews = fiscalViews;

            return View(fiscalHomePageView);
        }

        #endregion

        #region Create

        public ActionResult Create(string id)
        {
            FiscalDetailView fiscalDetailView = new FiscalDetailView();
            fiscalDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(fiscalDetailView);
            }
            #endregion
            
            #region Customer Info

            fiscalDetailView.FiscalView = new FiscalView();
            fiscalDetailView.FiscalView.CustomerID = Guid.Parse(id);
            GetRequest request = new GetRequest() { ID = Guid.Parse(id) };
            fiscalDetailView.CustomerView = GetCustomer(id);

            #endregion

            #region DropDownList For MoneyAccounts

            fiscalDetailView.MoneyAccountViews = _moneyAccountService.GetMoneyAccounts().MoneyAccountViews;
            List<DropDownItem> mnyList = new List<DropDownItem>();

            if (fiscalDetailView.MoneyAccountViews != null)
                foreach (MoneyAccountView moneyAccountView in fiscalDetailView.MoneyAccountViews)
                {
                    mnyList.Add(new DropDownItem { Value = moneyAccountView.ID.ToString(), Text = moneyAccountView.AccountName });
                }
            var mnySelectList = new SelectList(mnyList, "Value", "Text");
            ViewData["MoneyAccounts"] = mnySelectList;

            #endregion

            #region DropdownList For DocumentType

            List<DropDownItem> docType = new List<DropDownItem>();

            docType.Add(new DropDownItem { Value = "1", Text = "رسید عابربانک" });
            docType.Add(new DropDownItem { Value = "2", Text = "فیش بانکی" });
            docType.Add(new DropDownItem { Value = "3", Text = "چک" });
            docType.Add(new DropDownItem { Value = "4", Text = "قبض صندوق" });
            docType.Add(new DropDownItem { Value = "5", Text = "سایر" });

            var docSelectList = new SelectList(docType, "Value", "Text");
            ViewData["DocumentType"] = docSelectList;

            #endregion

            return View(fiscalDetailView);
        }

        [HttpPost]
        public ActionResult Create(FiscalDetailView fiscalDetailView)
        {
            fiscalDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Insert");
            if (!hasPermission)
            {
                return View(fiscalDetailView);
            }
            #endregion

            #region Customer Info

            GetRequest customerRequest = new GetRequest() { ID = fiscalDetailView.FiscalView.CustomerID };
            fiscalDetailView.CustomerView = _customerService.GetCustomer(customerRequest).CustomerView;

            #endregion

            #region DropDownList For MoneyAccounts

            fiscalDetailView.MoneyAccountViews = _moneyAccountService.GetMoneyAccounts().MoneyAccountViews;
            List<DropDownItem> mnyList = new List<DropDownItem>();

            if (fiscalDetailView.MoneyAccountViews != null)
                foreach (MoneyAccountView moneyAccountView in fiscalDetailView.MoneyAccountViews)
                {
                    mnyList.Add(new DropDownItem { Value = moneyAccountView.ID.ToString(), Text = moneyAccountView.AccountName });
                }
            var mnySelectList = new SelectList(mnyList, "Value", "Text");
            ViewData["MoneyAccounts"] = mnySelectList;

            #endregion

            #region DropdownList For DocumentType

            List<DropDownItem> docType = new List<DropDownItem>();

            docType.Add(new DropDownItem { Value = "1", Text = "رسید عابربانک" });
            docType.Add(new DropDownItem { Value = "2", Text = "فیش بانکی" });
            docType.Add(new DropDownItem { Value = "3", Text = "چک" });
            docType.Add(new DropDownItem { Value = "4", Text = "قبض صندوق" });
            docType.Add(new DropDownItem { Value = "5", Text = "سایر" });

            var docSelectList = new SelectList(docType, "Value", "Text");
            ViewData["DocumentType"] = docSelectList;

            #endregion

            if (ModelState.IsValid)
                try
                {
                    if (fiscalDetailView.FiscalView.Cost == 0)
                    {
                        ModelState.AddModelError("", "مبلغ مالی نمی تواند صفر باشد.");
                        return View(fiscalDetailView);
                    }

                    AddFiscalRequest request = new AddFiscalRequest();
                    request.CreateEmployeeID = GetEmployee().ID;

                    // Need For check Pey/Receipt to be match with TypeForCreate:
                    MoneyAccountView moneyAccountView = _moneyAccountService.GetMoneyAccount(new GetRequest() { ID = fiscalDetailView.FiscalView.MoneyAccountID }).MoneyAccountView;

                    if (fiscalDetailView.FiscalView.TypeForCreate == "دریافت از مشتری")
                    {
                        #region Check if MoneyAccount Pay/Receipt match with TypeForCreate

                        if (!moneyAccountView.Receipt)
                        {
                            ModelState.AddModelError("", "حساب پولی انتخاب شده مربوط به دریافت نمی باشد.");
                            return View(fiscalDetailView);
                        }

                        #endregion

                        request.Cost = Math.Abs(fiscalDetailView.FiscalView.Cost);
                    }
                    else
                    {
                        #region Check if MoneyAccount Pay/Receipt match with TypeForCreate

                        if (!moneyAccountView.Pay)
                        {
                            ModelState.AddModelError("", "حساب پولی انتخاب شده مربوط به پرداخت نمی باشد.");
                            return View(fiscalDetailView);
                        }

                        #endregion

                        request.Cost = -Math.Abs(fiscalDetailView.FiscalView.Cost);
                    }
                    request.Note = fiscalDetailView.FiscalView.Note;
                    request.CustomerID = fiscalDetailView.CustomerView.ID;
                    request.DocumentSerial = fiscalDetailView.FiscalView.DocumentSerial;
                    request.DocumentType = fiscalDetailView.FiscalView.DocumentType;
                    request.InvestDate = fiscalDetailView.FiscalView.InvestDate;
                    request.MoneyAccountID = fiscalDetailView.FiscalView.MoneyAccountID;

                    GeneralResponse response = this._fiscalService.AddFiscal(request);

                    if (response.success)
                        return RedirectToAction("CustomerFiscal/" + fiscalDetailView.CustomerView.ID);
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(fiscalDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(fiscalDetailView);
                }

            return View(fiscalDetailView);
        }

        #endregion

        #region Edit

        public ActionResult Edit(string id)
        {
            FiscalDetailView fiscalDetailView = new FiscalDetailView();
            fiscalDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(fiscalDetailView);
            }
            #endregion

            
            fiscalDetailView.FiscalView = this.GetFiscalView(id);
            

            fiscalDetailView.FiscalView.TypeForCreate = fiscalDetailView.FiscalView.Cost >= 0 ? "دریافت از مشتری" : "پرداخت به مشتری";
            fiscalDetailView.FiscalView.Cost = Math.Abs(fiscalDetailView.FiscalView.Cost);

            #region Customer Info

            fiscalDetailView.CustomerView = GetCustomer(fiscalDetailView.FiscalView.CustomerID);

            #endregion

            #region DropDownList For MoneyAccounts

            fiscalDetailView.MoneyAccountViews = _moneyAccountService.GetMoneyAccounts().MoneyAccountViews;
            List<DropDownItem> mnyList = new List<DropDownItem>();

            if (fiscalDetailView.MoneyAccountViews != null)
                foreach (MoneyAccountView moneyAccountView in fiscalDetailView.MoneyAccountViews)
                {
                    mnyList.Add(new DropDownItem { Value = moneyAccountView.ID.ToString(), Text = moneyAccountView.AccountName });
                }
            var mnySelectList = new SelectList(mnyList, "Value", "Text");
            ViewData["MoneyAccounts"] = mnySelectList;

            #endregion

            #region DropdownList For DocumentType

            List<DropDownItem> docType = new List<DropDownItem>();

            docType.Add(new DropDownItem { Value = "1", Text = "رسید عابربانک" });
            docType.Add(new DropDownItem { Value = "2", Text = "فیش بانکی" });
            docType.Add(new DropDownItem { Value = "3", Text = "چک" });
            docType.Add(new DropDownItem { Value = "4", Text = "قبض صندوق" });
            docType.Add(new DropDownItem { Value = "5", Text = "سایر" });

            var docSelectList = new SelectList(docType, "Value", "Text");
            ViewData["DocumentType"] = docSelectList;

            #endregion

            return View(fiscalDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, FiscalDetailView fiscalDetailView)
        {
            fiscalDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(fiscalDetailView);
            }
            #endregion
            #region Customer Info

            GetRequest customerRequest = new GetRequest() { ID = fiscalDetailView.FiscalView.CustomerID };
            fiscalDetailView.CustomerView = _customerService.GetCustomer(customerRequest).CustomerView;

            #endregion

            #region DropDownList For MoneyAccounts

            fiscalDetailView.MoneyAccountViews = _moneyAccountService.GetMoneyAccounts().MoneyAccountViews;
            List<DropDownItem> mnyList = new List<DropDownItem>();

            if (fiscalDetailView.MoneyAccountViews != null)
                foreach (MoneyAccountView moneyAccountView in fiscalDetailView.MoneyAccountViews)
                {
                    mnyList.Add(new DropDownItem { Value = moneyAccountView.ID.ToString(), Text = moneyAccountView.AccountName });
                }
            var mnySelectList = new SelectList(mnyList, "Value", "Text");
            ViewData["MoneyAccounts"] = mnySelectList;

            #endregion

            #region DropdownList For DocumentType

            List<DropDownItem> docType = new List<DropDownItem>();

            docType.Add(new DropDownItem { Value = "1", Text = "رسید عابربانک" });
            docType.Add(new DropDownItem { Value = "2", Text = "فیش بانکی" });
            docType.Add(new DropDownItem { Value = "3", Text = "چک" });
            docType.Add(new DropDownItem { Value = "4", Text = "قبض صندوق" });
            docType.Add(new DropDownItem { Value = "5", Text = "سایر" });

            var docSelectList = new SelectList(docType, "Value", "Text");
            ViewData["DocumentType"] = docSelectList;

            #endregion

            if (ModelState.IsValid)
                try
                {
                    // Cost Can not accept zero(0)
                    if (fiscalDetailView.FiscalView.Cost == 0)
                    {
                        ModelState.AddModelError("", "مبلغ مالی نمی تواند صفر باشد.");
                        return View(fiscalDetailView);
                    }

                    EditFiscalRequest request = new EditFiscalRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;

                    // Need For check Pey/Receipt to be match with TypeForCreate:
                    MoneyAccountView moneyAccountView = _moneyAccountService.GetMoneyAccount(new GetRequest() { ID = fiscalDetailView.FiscalView.MoneyAccountID }).MoneyAccountView;

                    if (fiscalDetailView.FiscalView.TypeForCreate == "دریافت از مشتری")
                    {
                        #region Check if MoneyAccount Pay/Receipt match with TypeForCreate

                        if (!moneyAccountView.Receipt)
                        {
                            ModelState.AddModelError("", "حساب پولی انتخاب شده مربوط به دریافت نمی باشد.");
                            return View(fiscalDetailView);
                        }

                        #endregion

                        request.Cost = Math.Abs(fiscalDetailView.FiscalView.Cost);
                    }
                    else
                    {
                        #region Check if MoneyAccount Pay/Receipt match with TypeForCreate

                        if (!moneyAccountView.Pay)
                        {
                            ModelState.AddModelError("", "حساب پولی انتخاب شده مربوط به پرداخت نمی باشد.");
                            return View(fiscalDetailView);
                        }

                        #endregion

                        request.Cost = -Math.Abs(fiscalDetailView.FiscalView.Cost);
                    }
                    request.Note = fiscalDetailView.FiscalView.Note;
                    request.DocumentSerial = fiscalDetailView.FiscalView.DocumentSerial;
                    request.DocumentType = fiscalDetailView.FiscalView.DocumentType;
                    request.InvestDate = fiscalDetailView.FiscalView.InvestDate;
                    request.MoneyAccountID = fiscalDetailView.FiscalView.MoneyAccountID;
                    request.RowVersion = fiscalDetailView.FiscalView.RowVersion;

                    GeneralResponse response = this._fiscalService.EditFiscal(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(fiscalDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(fiscalDetailView);
                }

            return View(fiscalDetailView);
        }

        #endregion

        #region Details

        public ActionResult Details(string id)
        {
            FiscalDetailView fiscalDetailView = new FiscalDetailView();
            fiscalDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(fiscalDetailView);
            }
            #endregion
            
            FiscalView fiscalView = this.GetFiscalView(id);

            fiscalDetailView = new FiscalDetailView();
            fiscalDetailView.FiscalView = fiscalView;
            
            return View(fiscalDetailView);
        }
        #endregion

        #region Delete

        public ActionResult Delete(string id)
        {
            FiscalDetailView fiscalDetailView = new FiscalDetailView();
            fiscalDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(fiscalDetailView);
            }
            #endregion

            fiscalDetailView.FiscalView = this.GetFiscalView(id);
            
            return View(fiscalDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            FiscalDetailView fiscalDetailView = new FiscalDetailView();
            fiscalDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(fiscalDetailView);
            }
            #endregion

            fiscalDetailView.FiscalView = this.GetFiscalView(id);
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._fiscalService.DeleteFiscal(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(fiscalDetailView);
            }
        }
        #endregion

        #region Confirm

        public ActionResult ConfirmFiscal(string id)
        {
            FiscalDetailView fiscalDetailView = new FiscalDetailView();
            fiscalDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_DoConfirm");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(fiscalDetailView);
            }
            #endregion

            fiscalDetailView.FiscalView = this.GetFiscalView(id);
            
            #region Customer Info

            fiscalDetailView.CustomerView = GetCustomer(fiscalDetailView.FiscalView.CustomerID);

            #endregion

            #region DropdownList For ConfirmEnum

            List<DropDownItem> confirmEnumDropdown = new List<DropDownItem>();

            confirmEnumDropdown.Add(new DropDownItem { Value = "1", Text = "بررسی نشده" });
            confirmEnumDropdown.Add(new DropDownItem { Value = "2", Text = "تأیید شد" });
            confirmEnumDropdown.Add(new DropDownItem { Value = "3", Text = "تأیید نشد" });

            var cnfSelectList = new SelectList(confirmEnumDropdown, "Value", "Text");
            ViewData["Confirm"] = cnfSelectList;

            #endregion

            return View(fiscalDetailView);
        }

        [HttpPost]
        public ActionResult ConfirmFiscal(string id, FiscalDetailView fiscalDetailView)
        {
            // مقدار قدیم تراکنش مالی
            // اگر در حین عملیات خطایی رخ داد این مقدار را برمی گردانیم
            FiscalDetailView oldFiscalDetailView = new FiscalDetailView();
            oldFiscalDetailView.EmployeeView = GetEmployee();


            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_DoConfirm");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(oldFiscalDetailView);
            }
            #endregion

            oldFiscalDetailView.FiscalView = GetFiscalView(id);

            #region Customer Info

            oldFiscalDetailView.CustomerView = GetCustomer(oldFiscalDetailView.FiscalView.CustomerID);

            #endregion

            #region DropdownList For ConfirmEnum

            List<DropDownItem> confirmEnumDropdown = new List<DropDownItem>();

            confirmEnumDropdown.Add(new DropDownItem { Value = "1", Text = "بررسی نشده" });
            confirmEnumDropdown.Add(new DropDownItem { Value = "2", Text = "تأیید شد" });
            confirmEnumDropdown.Add(new DropDownItem { Value = "3", Text = "تأیید نشد" });

            var cnfSelectList = new SelectList(confirmEnumDropdown, "Value", "Text");
            ViewData["Confirm"] = cnfSelectList;

            #endregion

            if (ModelState.IsValid)
                try
                {
                    ConfirmRequest request = new ConfirmRequest();
                    request.FiscalID = Guid.Parse(id);
                    request.ConfirmEmployeeID = GetEmployee().ID;
                    request.Confirm = fiscalDetailView.FiscalView.Confirm;
                    request.RowVersion = fiscalDetailView.FiscalView.RowVersion;

                    // if not confirmed, the confirmedCost most be 0
                    if (fiscalDetailView.FiscalView.Confirm == ConfirmEnum.Confirmed)
                    {
                        // Check sign of Cost for confirm
                        if (oldFiscalDetailView.FiscalView.Cost > 0)
                            request.ConfirmedCost = Math.Abs(fiscalDetailView.FiscalView.ConfirmedCost);
                        else
                            if (oldFiscalDetailView.FiscalView.Cost < 0)
                                request.ConfirmedCost = -Math.Abs(fiscalDetailView.FiscalView.ConfirmedCost);
                            else
                            {
                                ModelState.AddModelError("", "مبلغ تأیید شده نمی تواند صفر باشد.");
                                return View(oldFiscalDetailView);
                            }
                    }
                    else if (fiscalDetailView.FiscalView.Confirm == ConfirmEnum.NotConfirmed)
                    {
                        request.ConfirmedCost = 0;
                    }
                    else
                    {
                        ModelState.AddModelError("", "در صورتی اطلاعات شما ذخیره می شود که تأیید شد یا تأیید نشد را انتخاب کنید.");
                        return View(oldFiscalDetailView);
                    }

                    GeneralResponse response = _fiscalService.Confirm(request);

                    if (response.success)
                        return RedirectToAction("CustomerFiscal/" + oldFiscalDetailView.CustomerView.ID);
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(oldFiscalDetailView);
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(oldFiscalDetailView);
                }

            return View(oldFiscalDetailView);
        }

        #endregion

        #region Home

        public ActionResult Home()
        {
            HomePageView homePageView = new HomePageView();
            homePageView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(homePageView);
            }
            #endregion

            return View(homePageView);
        }

        #endregion

        #endregion

        #region Json Methods

        public JsonResult Fiscals_Read(Guid? customerID, int? pageSize, int? pageNumber,string sort,IList<FilterData> filter)
        {
            GetGeneralResponse<IEnumerable<FiscalView>> response = new GetGeneralResponse<IEnumerable<FiscalView>>();

            //#region Check Access
            //bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Read");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
            //#endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            #region کثافت کاری فیلتر

            IList<FilterData> Filters = new List<FilterData>();
            if(filter!=null)
            foreach (var item in filter)
            {
                if (item.data.type == "list")
                {
                    if (item.data.value.Contains("دریافت"))
                    {
                        FilterData _filter = new FilterData()
                        {
                            field = "Cost",
                            data = new data()
                            {
                                comparison = "gt",
                                type = "numeric",
                                value = new[] { "0" }
                            }
                        };
                        Filters.Add(_filter);
                    }

                    else if (item.data.value.Contains("پرداخت"))
                    {
                        FilterData _filter = new FilterData()
                        {
                            field = "Cost",
                            data = new data()
                            {
                                comparison = "lt",
                                type = "numeric",
                                value = new[] { "0" }
                            }
                        };
                        Filters.Add(_filter);
                    }
                    else
                        Filters.Add(item);
                }
                else
                    Filters.Add(item);
            }

            #endregion

            if (customerID != null)
            {
                Guid CustomerID = customerID==null?Guid.Empty:(Guid) customerID;
                response = _fiscalService.GetFiscals(CustomerID, PageSize, PageNumber, ConvertJsonToObject(sort),Filters);
            }

            else
                response = _fiscalService.GetAllFiscals(PageSize, PageNumber, ConvertJsonToObject(sort), Filters);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #region get All Fiscals


        public JsonResult All_Fiscals_Read(int? pageSize, int? pageNumber, string sort,IList<FilterData> filter)
        {
            GetGeneralResponse<IEnumerable<FiscalView>> response = new GetGeneralResponse<IEnumerable<FiscalView>>();

            //#region Check Access
            //bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Read");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
            //#endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _fiscalService.GetAllFiscals(PageSize, PageNumber, ConvertJsonToObject(sort),filter);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion


        public JsonResult Fiscal_Delete(Guid fiscalID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _fiscalService.DeleteFiscal(new DeleteRequest() { ID = fiscalID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Fiscal_Insert(Int64 cost, bool isReceipt, Guid customerID, string documentSerial,
            int documentType, string note, Guid moneyAccountID, string investDate)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            AddFiscalRequest request = new AddFiscalRequest();
            
            request.CreateEmployeeID = GetEmployee().ID;
            request.Cost = isReceipt ? Math.Abs(cost) : -Math.Abs(cost);
            request.CustomerID = customerID;
            request.DocumentSerial = documentSerial;
            request.DocumentType = (DocType)documentType;
            request.Note = note;
            request.MoneyAccountID = moneyAccountID;
            request.InvestDate = investDate;

            response = this._fiscalService.AddFiscal(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Fiscal_Edit(Guid fiscalID, Int64 cost, bool isReceipt, string documentSerial,
            int documentType, string note, Guid moneyAccountID, string investDate, int rowVersion )
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            EditFiscalRequest request = new EditFiscalRequest();
            request.ID = fiscalID;
            request.RowVersion = rowVersion;
            request.ModifiedEmployeeID = GetEmployee().ID;
            request.Cost = isReceipt ? Math.Abs(cost) : -Math.Abs(cost);
            request.DocumentSerial = documentSerial;
            request.DocumentType = (DocType)documentType;
            request.Note = note;
            request.MoneyAccountID = moneyAccountID;
            request.InvestDate = investDate;

            response = this._fiscalService.EditFiscal(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Fiscal_DoConfirm(Guid fiscalID, int confirm, int confirmedCost, int rowVersion, bool? ForCharge, long fiscalReciptNumber)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Fiscal_DoConfirm");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            ConfirmRequest request = new ConfirmRequest();
            request.FiscalID = fiscalID;
            request.ConfirmEmployeeID = GetEmployee().ID;
            request.Confirm = (ConfirmEnum)confirm;
            request.ConfirmedCost = confirmedCost;
            request.RowVersion = rowVersion;
            request.FiscalReciptNumber = fiscalReciptNumber;
            response = _fiscalService.Confirm(request);
            
            if ((confirm != 1 && confirm !=3) && ForCharge==true && response.ErrorMessages.Count()<1)
            {
                #region تغییر مرحله مشتری به شارژ و خدمات ثانویه

                GetGeneralResponse<FiscalView> fiscalResponse = new GetGeneralResponse<FiscalView>();
                fiscalResponse = _fiscalService.GetFiscalByID(fiscalID);

                //GetCustomerResponse customer = _customerService.GetCustomer(new GetRequest() { ID = fiscalResponse.data.CustomerID });


                AddCustomerLevelRequest levelRequest = new AddCustomerLevelRequest();
                levelRequest.CreateEmployeeID = GetEmployee().ID;
                levelRequest.CustomerID = fiscalResponse.data.CustomerID;
                levelRequest.NewLevelID = Guid.Parse("5D6457D7-C23F-4614-AF4B-93257ABAC056");
                levelRequest.Note = "تغییر مرحه توسط ثبت مالی";

                _customerLevelService.AddCustomerLevel(levelRequest);

                #endregion
            }

            

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Charge_DoConfirm(int chargStatus, Guid fiscalID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Charge_DoConfirm");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _fiscalService.ChangeCharedStatus((ChargeStatus)chargStatus, fiscalID, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region Fiscall Add By VOIP

        public JsonResult Fiscal_ByVoip_Insert(string customer_phone, string deposit_date, bool IsMellat, long amount, string bandwith,
    string volume, string duration, int deposit_type, string receipt_number, bool forCharge,string cardNumber)
        {
            GeneralResponse response = new GeneralResponse();

            AddFiscalRequest request =new  AddFiscalRequest();

            request.Phone = customer_phone;
            request.InvestDate = deposit_date;
            request.MoneyAccountID = IsMellat == true ? Guid.Parse("0AD677AC-BBD2-4C11-83C4-00DCB49D353D") : Guid.Parse("A4039515-23FC-4BF6-8CC8-8CAA250899B1");
            request.Cost = amount;
            request.Note = String.Format("سرویس {0} کیلو بایت {1} ماهه {2} گیگابایت ترافیک و شماره کارت :{3} میباشد", bandwith, duration, volume,cardNumber);
            request.DocumentType = (DocType)deposit_type;
            request.DocumentSerial = receipt_number;
            request.ForCharge = forCharge;
            request.CreateEmployeeID = GetEmployee().ID;
            

            response = _fiscalService.AddFiscal(request);

            long res = -1;
            if (!response.hasError)
            {
                FiscalView fiscalView = (FiscalView)response.ObjectAdded;
                res = fiscalView.FollowNumber;
                return Json(res, JsonRequestBehavior.AllowGet);
            }

            else
            {
                response.ErrorMessages.Add("Error");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            
        }

        // TODO: @Zafari do this method:
        public JsonResult Fiscal_Followup(int follow_number)
        {
            GetGeneralResponse<FiscalView> response = new GetGeneralResponse<FiscalView>();

            response = _fiscalService.GetFollowUpNumber(follow_number);

            return Json(response.data.ChargeStatus, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Private Members

        private FiscalView GetFiscalView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetFiscalResponse response = this._fiscalService.GetFiscal(request);

            return response.FiscalView;
        }

        #endregion

        
    }
}
