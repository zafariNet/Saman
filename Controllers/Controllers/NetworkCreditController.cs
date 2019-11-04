using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.StoreCatalog;
using System.Web.Mvc;
//using System.Web.UI.WebControls;
using Services.ViewModels.Store;
using Services.Messaging;
using Services.Messaging.StoreCatalogService;
using Controllers.ViewModels;
using Services.ViewModels.Fiscals;

namespace Controllers.Controllers
{
    [Authorize]
    public class NetworkCreditController: BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly INetworkCreditService _networkCreditService;
        private readonly INetworkService _networkService;
        private readonly IMoneyAccountService _moneyAccountService;
        #endregion

        #region Ctor
        public NetworkCreditController(IEmployeeService employeeService, INetworkCreditService networkCreditService
            , INetworkService networkService
            , IMoneyAccountService moneyAccountService)
            : base(employeeService)
        {
            this._networkCreditService = networkCreditService;
            this._networkService = networkService;
            this._employeeService = employeeService;
            _moneyAccountService = moneyAccountService;
        }
        #endregion

        #region Old Methods

        #region Index
        public ActionResult Index(string id)
        {
            NetworkCreditHomePageView networkCreditHomePageView = new NetworkCreditHomePageView();
            networkCreditHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("NetworkCredit_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkCreditHomePageView);
            }
            #endregion

            

            GetNetworkCreditsRequest request = new GetNetworkCreditsRequest() { NetworkID = Guid.Parse(id) };
            networkCreditHomePageView.NetworkCreditViews =
                _networkCreditService.GetNetworkCredits(request).NetworkCreditViews.OrderByDescending(o => o.TransactionDate);
            networkCreditHomePageView.NetworkVeiw = _networkService.GetNetwork(new GetRequest() { ID = Guid.Parse(id) }).NetworkView;

            return View(networkCreditHomePageView);
        }
        #endregion

        #region Create
        public ActionResult Create(string id)
        {
            NetworkCreditDetailView networkCreditDetailView = new NetworkCreditDetailView();
            networkCreditDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("NetworkCredit_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkCreditDetailView);
            }
            #endregion

            
            networkCreditDetailView.NetworkCreditView = new NetworkCreditView();
            networkCreditDetailView.NetworkCreditView.NetworkID = Guid.Parse(id);
            networkCreditDetailView.NetworkCreditView.NetworkName = _networkService.GetNetwork(new GetRequest() { ID = Guid.Parse(id) }).NetworkView.NetworkName;

            #region DropDownList For MoneyAccounts
            networkCreditDetailView.MoneyAccountViews = _moneyAccountService.GetBankAccounts().MoneyAccountViews;
            List<DropDownItem> list = new List<DropDownItem>();
            list.Add(new DropDownItem { Value = Guid.Empty, Text = "انتخاب نشده" });

            foreach (MoneyAccountView moneyAccountView in networkCreditDetailView.MoneyAccountViews)
            {
                list.Add(new DropDownItem { Value = moneyAccountView.ID.ToString(), Text = moneyAccountView.AccountName });
            }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["BankAccounts"] = selectList;
            #endregion

            return View(networkCreditDetailView);
        }

        [HttpPost]
        public ActionResult Create(NetworkCreditDetailView networkCreditDetailView)
        {
            networkCreditDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("NetworkCredit_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkCreditDetailView);
            }
            #endregion

            #region DropDownList For MoneyAccounts
            networkCreditDetailView.MoneyAccountViews = _moneyAccountService.GetBankAccounts().MoneyAccountViews;
            List<DropDownItem> list = new List<DropDownItem>();
            list.Add(new DropDownItem { Value = null, Text = "انتخاب نشده" });

            foreach (MoneyAccountView moneyAccountView in networkCreditDetailView.MoneyAccountViews)
            {
                list.Add(new DropDownItem { Value = moneyAccountView.ID.ToString(), Text = moneyAccountView.AccountName });
            }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["BankAccounts"] = selectList;
            #endregion

            

            if (ModelState.IsValid)
                try
                {
                    AddNetworkCreditRequestOld request = new AddNetworkCreditRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    //Edited By Zafari
                    //Orginal if (networkCreditDetailView.NetworkCreditView.TypeForCreate == "برداشت")
                    if (networkCreditDetailView.NetworkCreditView.TypeForCreate == "B")
                        request.Amount = -Math.Abs(networkCreditDetailView.NetworkCreditView.Amount);
                    else
                        request.Amount = Math.Abs(networkCreditDetailView.NetworkCreditView.Amount);
                    request.FromAccountID = networkCreditDetailView.NetworkCreditView.FromAccountID;
                    request.InvestDate = networkCreditDetailView.NetworkCreditView.InvestDate;
                    request.NetworkID = networkCreditDetailView.NetworkCreditView.NetworkID;
                    request.ToAccount = networkCreditDetailView.NetworkCreditView.ToAccount;
                    request.Note = networkCreditDetailView.NetworkCreditView.Note;
                    request.TransactionNo = networkCreditDetailView.NetworkCreditView.TransactionNo;

                    GeneralResponse response = this._networkCreditService.AddNetworkCredit(request);

                    if (response.success)
                        return RedirectToAction("Index/" + request.NetworkID);
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(networkCreditDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(networkCreditDetailView);
                }

            return View(networkCreditDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            NetworkCreditDetailView networkCreditDetailView = new NetworkCreditDetailView();
            networkCreditDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("NetworkCredit_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkCreditDetailView);
            }
            #endregion

            networkCreditDetailView.NetworkCreditView = this.GetNetworkCreditView(id);
            networkCreditDetailView.NetworkCreditView.TypeForCreate = networkCreditDetailView.NetworkCreditView.Amount >= 0 ? "واریز" : "برداشت";
            networkCreditDetailView.NetworkCreditView.Amount = Math.Abs(networkCreditDetailView.NetworkCreditView.Amount);

            #region DropDownList For MoneyAccounts
            networkCreditDetailView.MoneyAccountViews = _moneyAccountService.GetBankAccounts().MoneyAccountViews;
            List<DropDownItem> list = new List<DropDownItem>();
            list.Add(new DropDownItem { Value = Guid.Empty, Text = "انتخاب نشده" });

            foreach (MoneyAccountView moneyAccountView in networkCreditDetailView.MoneyAccountViews)
            {
                list.Add(new DropDownItem { Value = moneyAccountView.ID.ToString(), Text = moneyAccountView.AccountName });
            }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["BankAccounts"] = selectList;
            #endregion

            return View(networkCreditDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, NetworkCreditDetailView networkCreditDetailView)
        {
            networkCreditDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("NetworkCredit_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkCreditDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    #region DropDownList For MoneyAccounts
                    networkCreditDetailView.MoneyAccountViews = _moneyAccountService.GetBankAccounts().MoneyAccountViews;
                    List<DropDownItem> list = new List<DropDownItem>();
                    list.Add(new DropDownItem { Value = Guid.Empty, Text = "انتخاب نشده" });

                    foreach (MoneyAccountView moneyAccountView in networkCreditDetailView.MoneyAccountViews)
                    {
                        list.Add(new DropDownItem { Value = moneyAccountView.ID.ToString(), Text = moneyAccountView.AccountName });
                    }
                    var selectList = new SelectList(list, "Value", "Text");
                    ViewData["BankAccounts"] = selectList;
                    #endregion

                    EditNetworkCreditRequestOld request = new EditNetworkCreditRequestOld();
                    networkCreditDetailView.NetworkCreditView.Amount = Math.Abs(networkCreditDetailView.NetworkCreditView.Amount);

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    if (networkCreditDetailView.NetworkCreditView.TypeForCreate == "برداشت")
                        request.Amount = -networkCreditDetailView.NetworkCreditView.Amount;
                    else
                        request.Amount = networkCreditDetailView.NetworkCreditView.Amount;
                    request.FromAccountID = networkCreditDetailView.NetworkCreditView.FromAccountID;
                    request.InvestDate = networkCreditDetailView.NetworkCreditView.InvestDate;
                    request.NetworkID = networkCreditDetailView.NetworkCreditView.NetworkID;
                    request.ToAccount = networkCreditDetailView.NetworkCreditView.ToAccount;
                    request.Note = networkCreditDetailView.NetworkCreditView.Note;
                    request.TransactionNo = networkCreditDetailView.NetworkCreditView.TransactionNo;
                    request.RowVersion = networkCreditDetailView.NetworkCreditView.RowVersion;

                    GeneralResponse response = this._networkCreditService.EditNetworkCredit(request);

                    if (response.success)
                        return RedirectToAction("Index/" + request.NetworkID);
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(networkCreditDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(networkCreditDetailView);
                }

            return View(networkCreditDetailView);
        }
        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            NetworkCreditDetailView networkCreditDetailView = new NetworkCreditDetailView();
            networkCreditDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("NetworkCredit_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkCreditDetailView);
            }
            #endregion

            NetworkCreditView networkCreditView = this.GetNetworkCreditView(id);

            networkCreditDetailView.NetworkCreditView = networkCreditView;
            
            return View(networkCreditDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            NetworkCreditDetailView networkCreditDetailView = new NetworkCreditDetailView();
            networkCreditDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("NetworkCredit_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkCreditDetailView);
            }
            #endregion

            
            networkCreditDetailView.NetworkCreditView = this.GetNetworkCreditView(id);
            
            return View(networkCreditDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            NetworkCreditDetailView networkCreditDetailView = new NetworkCreditDetailView();
            networkCreditDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("NetworkCredit_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkCreditDetailView);
            }
            #endregion
            
            networkCreditDetailView.NetworkCreditView = this.GetNetworkCreditView(id);
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._networkCreditService.DeleteNetworkCredit(request);

            if (response.success)
                return RedirectToAction("Index/" + networkCreditDetailView.NetworkCreditView.NetworkID);
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(networkCreditDetailView);
            }
        }
        #endregion

        #endregion

        #region New Metods

        #region Read

        public JsonResult NetworkCredit_Read(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();


            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Network_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _networkCreditService.GetNetworkCredit(ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NetworkCredits_Read(Guid ID)
        {
            GetGeneralResponse<IEnumerable<NetworkCreditView>> response = new GetGeneralResponse<IEnumerable<NetworkCreditView>>();


            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Network_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _networkCreditService.GetNetworkCredits(ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult NetworkCredit_Insert(AddNetworkCreditRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Network_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _networkCreditService.AddNetworkCredit(request, GetEmployee().ID);
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult NetworkCredit_Update(EditNetworkCreditRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Network_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion


            response = _networkCreditService.EditNetworkCredit(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete

        public JsonResult NetworkCredits_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Network_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _networkCreditService.DeleteNetworkCredits(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #endregion

        #region Private Members

        private NetworkCreditView GetNetworkCreditView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetNetworkCreditResponse response = this._networkCreditService.GetNetworkCredit(request);

            return response.NetworkCreditView;
        }

        #endregion

    }
}
