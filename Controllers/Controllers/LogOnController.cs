#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels;
using System.Web.Mvc;
using System.Web.Security;
using Services.Messaging;
using Controllers.ViewModels.CustomerCatalog;
using Controllers.ViewModels.EmployeeCatalog;
using Services.ViewModels.Employees;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels;
using Services.Messaging.CustomerCatalogService;
using Model.Employees;
using Services.Messaging.FiscalCatalogService;
using Services.ViewModels.Fiscals;
using Model.Fiscals;
using Services.ViewModels.Customers;

#endregion

namespace Controllers.Controllers
{
    /// <summary>
    /// کلاس مربوط به لاگین
    /// </summary>
    public class LogOnController : Controller
    {
        #region Declares

        private readonly IMainMenuService _mainMenuService;

        private readonly IEmployeeService _employeeService;

        private readonly IQueryService _queryService;

        private readonly IFiscalService _fiscalservice;

        #endregion

        #region ctor

        public LogOnController(IMainMenuService mainMenuService, 
            IEmployeeService employeeService,
            IQueryService queryService , IFiscalService fiscalservice)
        {
            _mainMenuService = mainMenuService;
            _employeeService = employeeService;
            _queryService = queryService;
            _fiscalservice = fiscalservice;
        }

        #endregion

        #region Old Authentication

        #region Index (Log on)

        public ActionResult Index()
        {
            LogOnPageView logonPageView = new LogOnPageView();

            return View(logonPageView);
        }

        [HttpPost]
        public ActionResult Index(LogOnPageView logonPageView, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string Authenticate = _employeeService.Authenticate(logonPageView.LoginName, logonPageView.Password, logonPageView.RememberMe);
                switch (Authenticate)
                {
                    case "OK":
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                            return Redirect(returnUrl);
                        if (returnUrl == null)
                            return Redirect(@"/Saman/Home");
                        break;

                    case "PasswordInvalid":
                        ModelState.AddModelError("", "کلمه عبور اشتباه است.");
                        return View(logonPageView);

                    case "Discontinued":
                        ModelState.AddModelError("", "کاربر گرامی، حساب شما توسط مدیر غیر فعال شده است.");
                        return View(logonPageView);

                    case "UserNameInvalid":
                        ModelState.AddModelError("", "نام کاربری مورد نظر یافت نشد.");
                        return View(logonPageView);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(logonPageView);
        }

        #endregion

        #region LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index");
        }

        #endregion

        #endregion

        #region Json Login and Logout

        #region Login

        public JsonResult Login(string LoginName, string Password, bool? RememberMe)
        {
            GeneralResponse response = new GeneralResponse();

            bool rememberMe = RememberMe == null ? false : (bool)RememberMe;

            string Authenticate = _employeeService.Authenticate(LoginName, Password, rememberMe);
            switch (Authenticate)
            {
                case "OK":
                    break;

                case "PasswordInvalid":
                    response.ErrorMessages.Add("PasswordIsWorngKey");
                    break;

                case "Discontinued":
                    response.ErrorMessages.Add("YourAccountSusspendedKey");
                    break;

                case "UserNameInvalid":
                    response.ErrorMessages.Add("AccountNotFoundKey");
                    break;
            }

            // If we got this far, something failed, redisplay form
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Logout

        public JsonResult Logout()
        {
            GeneralResponse response = new GeneralResponse();

            FormsAuthentication.SignOut();
            Session.Clear();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region IsLogedIn

        public JsonResult IsLogedIn()
        {
            var response = new GetGeneralResponse<EmployeeLoginView>();

            //var ClientDate = DateTime.Parse(Request.Headers.Get("Date"));
            //if (ClientDate.Date != DateTime.Now.Date)
            //{
            //    response.ErrorMessages.Add("تاریخ سیستم شما با تاریخ سرور مطابقت ندارد . لطفا ابتدا تاریخ سیستم خود را اصلاح نمایید.");

            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
            
            bool IsAuthenticated = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;

            EmployeeLoginView employeeLoginView = new EmployeeLoginView();
            EmployeeView employeeView = new EmployeeView();

            employeeLoginView.IsLogedIn = IsAuthenticated;

            string employeeName = string.Empty;

            if (IsAuthenticated)
            {
                employeeView = GetEmployee();
                employeeLoginView.EmployeeName = employeeView.Name;
                employeeLoginView.Picture = employeeView.Photo;
                employeeLoginView.EmployeeID = employeeView.ID;
                Dictionary<string, bool> Permits = new Dictionary<string, bool>();
                foreach (var permit in employeeView.Permissions)
                    if (permit != null)
                    {
                        Permits.Add(permit.PermitKey, permit.Guaranteed);
                        employeeLoginView.Permits = Permits;
                    }
                
            }

            

            response.data = employeeLoginView;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region MainMenu Items Read

        [Authorize]
        public JsonResult MainMenuItems_Read(string parentMenuName)
        {
            MainMenusGetRequest request = new MainMenusGetRequest();
            request.ParentMenuName = parentMenuName;
            request.EmployeeID = GetEmployee().ID;

            GetMainMenusResponse mainMenus = new GetMainMenusResponse();

            if (request.ParentMenuName != "CustomersView")
            {
                mainMenus = _mainMenuService.GetMainMenus(request);
                mainMenus.success = true;
            }
            //  لینک منوی مشتریان
            else
            {
                IList<MainMenuView> mainMenuView = new List<MainMenuView>();

                // Get queries from database
                GetQueriesResponse queriesResponse = this._queryService.GetQueries(new GetQueriesRequest() { EmployeeID = GetEmployee().ID });

                // prepare data to send to client
                if (queriesResponse != null && queriesResponse.QueryViews != null)
                    foreach (var query in queriesResponse.QueryViews)
                    {
                        
                        mainMenuView.Add(new MainMenuView()
                        {

                            SubmenuName = query.Title  ,
                            SubmenuUrl = "Customer/Customers_Read?queryID=" + query.ID.ToString(),
                            xType = query.xType,
                            Icon = "Content/images/fam/customers.png",
                            ID = query.ID,
                            columns = new ColumnViews(query.Columns),
                            PreLoad=query.PreLoad,
                            CustomerCount=query.CustomerCount
                        });
                    }

                mainMenus.data = mainMenuView;
                mainMenus.success = true;
                mainMenus.TotalCount = queriesResponse.TotalCount;
            }
            return Json(mainMenus, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Employee

        public EmployeeView GetEmployee(string EmpID)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(EmpID);

            GetEmployeeResponse response = _employeeService.GetEmployee(request);

            EmployeeView employeeView = new EmployeeView();
            employeeView = response.EmployeeView;

            return employeeView;
        }

        protected EmployeeView GetEmployee()
        {
            EmployeeView _employeeView = new EmployeeView();
            if (User != null && User.Identity.Name != "")
            {
                _employeeView = GetEmployee(User.Identity.Name);
            }

            return _employeeView;
        }

        public JsonResult CurrentEmployee()
        {
            EmployeeView employeeView = GetEmployee();

            GetGeneralResponse<EmployeeView> response = new GetGeneralResponse<EmployeeView>();

            response.data = employeeView;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        

        public JsonResult RecreateSaPermissions()
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            //bool hasPermission = GetEmployee().IsGuaranteed("Permission_Insert");
            //if (!hasPermission)
            //{
            //    ModelState.AddModelError("", "AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
            #endregion

            response = _employeeService.RecreateSaPermissions();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RecreateGroupPermissions(Guid GroupID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            //bool hasPermission = GetEmployee().IsGuaranteed("Permission_Insert");
            //if (!hasPermission)
            //{
            //    ModelState.AddModelError("", "AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
            #endregion

            response = _employeeService.RecreateGroupPermissions(GroupID);
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region Voip Login

        public GeneralResponse VoipLogin(string LoginName, string Password, bool? RememberMe)
        {
            GeneralResponse response = new GeneralResponse();

            bool rememberMe = RememberMe == null ? false : (bool)RememberMe;

            string Authenticate = _employeeService.Authenticate(LoginName, Password, rememberMe);
            switch (Authenticate)
            {
                case "OK":
                    break;

                case "PasswordInvalid":
                    response.ErrorMessages.Add("PasswordIsWorngKey");
                    break;

                case "Discontinued":
                    response.ErrorMessages.Add("YourAccountSusspendedKey");
                    break;

                case "UserNameInvalid":
                    response.ErrorMessages.Add("AccountNotFoundKey");
                    break;
            }

            // If we got this far, something failed, redisplay form
            return response;
        }

        #endregion

    }
}
