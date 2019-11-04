#region Usings
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Controllers.ViewModels;
using Controllers.ViewModels.CustomerCatalog;
using Kendo.Mvc.UI;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Customers;
using Services.ViewModels.Employees;
using Services.ViewModels.Store;

using System.Web.Routing;
using Infrastructure.Querying;
using Model.Customers;
using System.Net.Sockets;
using System.Net;
using System.Text;
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class CustomerController : BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly ICustomerService _customerService;
        private readonly ICenterService _centerService;
        private readonly ILevelTypeService _levelTypeService;
        private readonly ILevelService _levelService;
        private readonly ILevelLevelService _levelLevelService;
        private readonly ICustomerLevelService _customerLevelService;
        private readonly IQueryService _queryService;
        private readonly IAgencyService _agencyService;
        private readonly INetworkService _networkService;
        private readonly ISuctionModeService _suctionModeService;
        private readonly IBuyPossibilityService _buyPossibilityService;
        private readonly IFollowStatusService _followStatusService;
        private readonly INoteService _noteService;

        #endregion

        #region Ctor

        public CustomerController(IEmployeeService employeeService, ICustomerService customerService
            , ICenterService centerService, ILevelTypeService levelTypeService
            , ILevelService levelService, ILevelLevelService levelLevelService
            , ICustomerLevelService customerLevelService, IQueryService queryService
            , IAgencyService agencyService, INetworkService networkService
            , ISuctionModeService suctionModeService
            , IBuyPossibilityService buyPossibilityService
            , IFollowStatusService followStatusService            
            , INoteService noteService)
            : base(employeeService)
        {
            _customerService = customerService;
            _employeeService = employeeService;
            _centerService = centerService;
            _levelTypeService = levelTypeService;
            _customerLevelService = customerLevelService;
            _queryService = queryService;
            _levelService = levelService;
            _levelLevelService = levelLevelService;
            _agencyService = agencyService;
            _networkService = networkService;
            _suctionModeService = suctionModeService;
            _buyPossibilityService = buyPossibilityService;
            _followStatusService = followStatusService;
            _noteService = noteService;
        }
        
        #endregion

        #region Old Methods

        #region Ajax

        public ActionResult Customer_Read([DataSourceRequest] DataSourceRequest request)
        {

            
            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Read");
            if (!hasPermission)
            {
                DataSource _result = new DataSource();
                ModelState.AddModelError("", "AccessDenied");
                return View(_result);
            }
            #endregion

            CustomerHomePageView customerHomePageView = new CustomerHomePageView();
            customerHomePageView.EmployeeView = GetEmployee();
            QuickSearchRequest getRequest = new QuickSearchRequest();

            getRequest.pageSize = request.PageSize == 0 ? 10 : request.PageSize;
            getRequest.pageNumber = request.Page;
            GetGeneralResponse<IEnumerable<CustomerView>> response = this._customerService.GetCustomers(getRequest);

            customerHomePageView.CustomerViews = response.data;
            customerHomePageView.Count = response.totalCount;

            var result = new DataSourceResult()
            {
                Data = response.data,
                Total = response.totalCount
            };


            return Json(result);
        }

        public JsonResult AjaxCenter(string adslPhone)
        {
            GetCenterInfoResponse centerInfo = _centerService.GetCenterInfo(adslPhone, 4);

            var result = new
            {
                success = centerInfo.hasCenter,
                CenterInfo = String.Format("مرکز مخابراتی: {0} - {1}: {2}", centerInfo.CenterName, "وضعیت پشتیبانی", centerInfo.Status)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Index
        public ActionResult Index()
        {
            CustomerHomePageView customerHomePageView = new CustomerHomePageView();
            //customerHomePageView.EmployeeView = GetEmployee();
            //AjaxGetRequest getRequest = new AjaxGetRequest();

            //getRequest.PageSize = 10;
            //getRequest.PageNumber = 1;

            //GetCustomersResponse customerResponse = this._customerService.GetCustomers(getRequest);

            //customerHomePageView.CustomerViews = customerResponse.data;
            //customerHomePageView.Count = customerResponse.Count;

            //DataSourceRequest request = new DataSourceRequest
            //{
            //    PageSize = 10,
            //    Page = 1
            //};

            //Customer_Read(request);

            return View(customerHomePageView);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            CustomerDetailView customerDetailView = new CustomerDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerDetailView);
            }
            #endregion

            
            customerDetailView.EmployeeView = GetEmployee();

            #region DropDownList For LevelType
            customerDetailView.LevelTypeViews = _levelTypeService.GetLevelTypes().LevelTypeViews;
            List<DropDownItem> list = new List<DropDownItem>();
            list.Add(null);

            if (customerDetailView.LevelTypeViews != null)
                foreach (LevelTypeView levelTypeView in customerDetailView.LevelTypeViews)
                {
                    list.Add(new DropDownItem { Value = levelTypeView.ID.ToString(), Text = levelTypeView.Title });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["LevelTypes"] = selectList;
            #endregion

            #region DropDownList For Level
            //customerDetailView.LevelViews = _levelService.GetLevels().LevelViews;
            List<DropDownItem> Tlist = new List<DropDownItem>();
            Tlist.Add(new DropDownItem { Value = "", Text = "-- نوع چرخه را انتخاب کنید --" });

            var TselectList = new SelectList(Tlist, "Value", "Text");
            ViewData["Levels"] = TselectList;
            #endregion

            return View(customerDetailView);
        }

        [HttpPost]
        public ActionResult Create(CustomerDetailView customerDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerDetailView);
            }
            #endregion

            customerDetailView.EmployeeView = GetEmployee();

            if (ModelState.IsValid)
                try
                {
                    AddCustomerRequest request = new AddCustomerRequest();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.Address = customerDetailView.CustomerView.Address;
                    request.ADSLPhone = customerDetailView.CustomerView.ADSLPhone;
                    request.AgencyID = customerDetailView.CustomerView.AgencyID;
                    request.BirthDate = customerDetailView.CustomerView.BirthDate;
                    request.CenterID = customerDetailView.CustomerView.CenterID;
                    request.Discontinued = customerDetailView.CustomerView.Discontinued;
                    request.DocumentStatusID = customerDetailView.CustomerView.DocumentStatusID;
                    request.Email = customerDetailView.CustomerView.Email;
                    request.FirstName = customerDetailView.CustomerView.FirstName;
                    request.LegalType = customerDetailView.CustomerView.LegalType;
                    request.Job = customerDetailView.CustomerView.Job;
                    request.LastName = customerDetailView.CustomerView.LastName;
                    request.Locked = customerDetailView.CustomerView.Locked;
                    request.LockEmployeeID = customerDetailView.CustomerView.LockEmployeeID;
                    request.LockNote = customerDetailView.CustomerView.LockNote;
                    request.Mobile1 = customerDetailView.CustomerView.Mobile1;
                    request.Mobile2 = customerDetailView.CustomerView.Mobile2;
                    request.NetworkID = customerDetailView.CustomerView.NetworkID;
                    request.Note = customerDetailView.CustomerView.Note;
                    request.Phone = customerDetailView.CustomerView.Phone;
                    //request.SentToPap = customerDetailView.CustomerView.SentToPap;
                    request.SFirstName = customerDetailView.CustomerView.SFirstName;
                    request.SLastName = customerDetailView.CustomerView.SLastName;
                    request.SuctionModeID = customerDetailView.CustomerView.SuctionModeID;

                    GeneralResponse response = _customerService.AddCustomer(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(customerDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(customerDetailView);
                }

            return View(customerDetailView);
        }
        #endregion

        #region QCreate

        public ActionResult QCreate()
        {
            
            return Create();
        }

        public ActionResult Createsuccess()
        {
            CustomerCreatesuccess customerCreatesuccess = new CustomerCreatesuccess();

            customerCreatesuccess.EmployeeView = GetEmployee();

            // بدست آوردن مشخصات مشتری ثبت شده
            if (TempData["CustomerResponse"] != null)
                customerCreatesuccess.CustomerView = (TempData["CustomerResponse"] as GeneralResponse).ObjectAdded as CustomerView;
            else
            {
                // در این صورت احتمالا کاربر بصورت دستی یو آر ال وارد کرده است
                customerCreatesuccess.TempDataIsNull = true;
                return View(customerCreatesuccess);
            }

            // بدست آوردن مشخصات مرحله مشتری
            customerCreatesuccess.AddLevelResponse = (TempData["CustomerLevelResponse"] as GeneralResponse);

            if (customerCreatesuccess.AddLevelResponse != null)
            {

                CustomerLevelView customerLevelView = customerCreatesuccess.AddLevelResponse.ObjectAdded as CustomerLevelView;
                GetRequest getLevelRequest = new GetRequest()
                {
                    ID = customerLevelView.LevelID
                };
                customerCreatesuccess.LevelView = _levelService.GetLevel(getLevelRequest).data;
            }

            return View(customerCreatesuccess);
        }

        [HttpPost]
        public ActionResult QCreate(CustomerDetailView customerDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_QInsert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerDetailView);
            }
            #endregion

            customerDetailView.EmployeeView = GetEmployee();

            if (ModelState.IsValid)
                try
                {
                    AddCustomerRequest customerRequest = new AddCustomerRequest();
                    customerRequest.CreateEmployeeID = GetEmployee().ID;
                    customerRequest.ADSLPhone = customerDetailView.CustomerView.ADSLPhone;
                    customerRequest.FirstName = customerDetailView.CustomerView.FirstName;
                    customerRequest.LastName = customerDetailView.CustomerView.LastName;
                    customerRequest.Mobile1 = customerDetailView.CustomerView.Mobile1;
                    if (!customerDetailView.CustomerView.SkipCenter)
                        try
                        {
                            customerRequest.CenterID = _centerService.GetCenterInfo(customerRequest.ADSLPhone, 4).CenterID;
                        }
                        catch
                        {
                            ModelState.AddModelError("", "هیچ مرکزی برای شماره مورد نظر شما در سیستم ثبت نشده است. در صورتی که تمایل دارید مشتری مورد نظر به هر حال ذخیره شود، لطفاً گزینه «مرکز خالی باشد» را انتخاب کنید.");
                            return View(customerDetailView);
                        }

                    customerRequest.LevelID = customerDetailView.CustomerView.LevelID;

                    GeneralResponse customerResponse = _customerService.AddCustomer(customerRequest);

                    // agar customer ba movafaghiat Add shavad:
                    if (customerResponse.success)
                    {
                        // agar karbar levele moshtari ra moshakhas karde bashad:
                        if (customerRequest.LevelID != Guid.Empty)
                        {
                            AddCustomerLevelRequest customerLevelRequest = new AddCustomerLevelRequest()
                            {
                                CreateEmployeeID = customerRequest.CreateEmployeeID,
                                CustomerID = customerResponse.ID,
                                NewLevelID = customerRequest.LevelID
                            };

                            GeneralResponse customerLevelResponse = _customerLevelService.AddCustomerLevel(customerLevelRequest);

                            TempData["CustomerLevelResponse"] = customerLevelResponse;
                        }

                        // dar har soorat natijeye add kardane moshtari be Createsuccess ersal mishavad:
                        TempData["CustomerResponse"] = customerResponse;

                        return RedirectToAction("Createsuccess");
                    }
                    else
                    {
                        foreach (string error in customerResponse.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(customerDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(customerDetailView);
                }

            return View(customerDetailView);
        }

        #region Get Cascade

        public JsonResult GetCascadeLevelTypes()
        {
            IList<LevelTypeView> levelTypeViews = new List<LevelTypeView>();
            levelTypeViews = _levelTypeService.GetLevelTypes().LevelTypeViews.ToList();

            return Json(levelTypeViews.Select(c => new { LevelTypeID = c.ID, Title = c.Title }), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            CustomerDetailView customerDetailView = new CustomerDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerDetailView);
            }
            #endregion

            customerDetailView.CustomerView = this.GetCustomerView(id);
            customerDetailView.EmployeeView = GetEmployee();

            #region Dropdown for Agency

            IEnumerable<AgencyView> agencyVeiws = _agencyService.GetAgencies().data;
            List<DropDownItem> aList = new List<DropDownItem>();
            aList.Add(null);

            foreach (AgencyView agencyVeiw in agencyVeiws)
            {
                aList.Add(new DropDownItem { Value = agencyVeiw.ID.ToString(), Text = agencyVeiw.AgencyName });
            }
            var aSelectList = new SelectList(aList, "Value", "Text");
            ViewData["Agencies"] = aSelectList;

            #endregion

            #region Dropdown for Network

            IEnumerable<NetworkView> networkVeiws = _networkService.GetNetworks().NetworkViews;
            List<DropDownItem> nList = new List<DropDownItem>();
            nList.Add(null);

            foreach (NetworkView networkVeiw in networkVeiws)
            {
                nList.Add(new DropDownItem { Value = networkVeiw.ID.ToString(), Text = networkVeiw.NetworkName });
            }
            var nSelectList = new SelectList(nList, "Value", "Text");
            ViewData["Networks"] = nSelectList;

            #endregion

            #region Dropdown for SuctionMode

            IEnumerable<SuctionModeView> suctionModeVeiws = _suctionModeService.GetSuctionModes().SuctionModeViews;
            List<DropDownItem> sList = new List<DropDownItem>();
            sList.Add(null);

            foreach (SuctionModeView suctionModeVeiw in suctionModeVeiws)
            {
                sList.Add(new DropDownItem { Value = suctionModeVeiw.ID.ToString(), Text = suctionModeVeiw.SuctionModeName });
            }
            var sSelectList = new SelectList(sList, "Value", "Text");
            ViewData["SuctionModes"] = sSelectList;

            #endregion

            return View(customerDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, CustomerDetailView customerDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerDetailView);
            }
            #endregion

            #region Dropdown for Agency

            IEnumerable<AgencyView> agencyVeiws = _agencyService.GetAgencies().data;
            List<DropDownItem> aList = new List<DropDownItem>();
            aList.Add(null);

            foreach (AgencyView agencyVeiw in agencyVeiws)
            {
                aList.Add(new DropDownItem { Value = agencyVeiw.ID.ToString(), Text = agencyVeiw.AgencyName });
            }
            var aSelectList = new SelectList(aList, "Value", "Text");
            ViewData["Agencies"] = aSelectList;

            #endregion

            #region Dropdown for Network

            IEnumerable<NetworkView> networkVeiws = _networkService.GetNetworks().NetworkViews;
            List<DropDownItem> nList = new List<DropDownItem>();
            nList.Add(null);

            foreach (NetworkView networkVeiw in networkVeiws)
            {
                nList.Add(new DropDownItem { Value = networkVeiw.ID.ToString(), Text = networkVeiw.NetworkName });
            }
            var nSelectList = new SelectList(nList, "Value", "Text");
            ViewData["Networks"] = nSelectList;

            #endregion

            #region Dropdown for SuctionMode

            IEnumerable<SuctionModeView> suctionModeVeiws = _suctionModeService.GetSuctionModes().SuctionModeViews;
            List<DropDownItem> sList = new List<DropDownItem>();
            sList.Add(null);

            foreach (SuctionModeView suctionModeVeiw in suctionModeVeiws)
            {
                sList.Add(new DropDownItem { Value = suctionModeVeiw.ID.ToString(), Text = suctionModeVeiw.SuctionModeName });
            }
            var sSelectList = new SelectList(sList, "Value", "Text");
            ViewData["SuctionModes"] = sSelectList;

            #endregion

            customerDetailView.EmployeeView = GetEmployee();

            //if (ModelState.IsValid)
            try
            {
                EditCustomerRequest request = new EditCustomerRequest();

                request.ID = Guid.Parse(id);
                request.ModifiedEmployeeID = GetEmployee().ID;
                request.Address = customerDetailView.CustomerView.Address;
                request.ADSLPhone = customerDetailView.CustomerView.ADSLPhone;
                request.AgencyID = customerDetailView.CustomerView.AgencyID;
                request.BirthDate = customerDetailView.CustomerView.BirthDate;
                request.CenterID = customerDetailView.CustomerView.CenterID;
                request.Discontinued = customerDetailView.CustomerView.Discontinued;
                request.DocumentStatusID = customerDetailView.CustomerView.DocumentStatusID;
                request.Email = customerDetailView.CustomerView.Email;
                request.FirstName = customerDetailView.CustomerView.FirstName;
                request.LegalType = customerDetailView.CustomerView.LegalType;
                request.Job = customerDetailView.CustomerView.Job;
                request.LastName = customerDetailView.CustomerView.LastName;
                request.Locked = customerDetailView.CustomerView.Locked;
                request.LockEmployeeID = customerDetailView.CustomerView.LockEmployeeID;
                request.LockNote = customerDetailView.CustomerView.LockNote;
                request.Mobile1 = customerDetailView.CustomerView.Mobile1;
                request.Mobile2 = customerDetailView.CustomerView.Mobile2;
                request.NetworkID = customerDetailView.CustomerView.NetworkID;
                request.Note = customerDetailView.CustomerView.Note;
                request.Phone = customerDetailView.CustomerView.Phone;
                //request.SentToPap = customerDetailView.CustomerView.SentToPap;
                request.SFirstName = customerDetailView.CustomerView.SFirstName;
                request.SLastName = customerDetailView.CustomerView.SLastName;
                request.SuctionModeID = customerDetailView.CustomerView.SuctionModeID;
                request.RowVersion = customerDetailView.CustomerView.RowVersion;

                GeneralResponse response = this._customerService.EditCustomer(request);

                if (response.success)
                    return RedirectToAction("Index");
                else
                {
                    foreach (string error in response.ErrorMessages)
                        ModelState.AddModelError("", error);
                    return View(customerDetailView);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(customerDetailView);
            }
        }
        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            CustomerDetailView customerDetailView = new CustomerDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerDetailView);
            }
            #endregion


            CustomerView customerView = this.GetCustomerView(id);

            customerDetailView.CustomerView = customerView;
            customerDetailView.EmployeeView = GetEmployee();

            return View(customerDetailView);
        }
        #endregion

        #region Delete

        public ActionResult Delete(string id)
        {
            CustomerDetailView customerDetailView = new CustomerDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerDetailView);
            }
            #endregion

            customerDetailView.CustomerView = this.GetCustomerView(id);
            customerDetailView.EmployeeView = GetEmployee();

            return View(customerDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            CustomerDetailView customerDetailView = new CustomerDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerDetailView);
            }
            #endregion

            customerDetailView.CustomerView = this.GetCustomerView(id);
            customerDetailView.EmployeeView = GetEmployee();

            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._customerService.DeleteCustomer(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(customerDetailView);
            }
        }

        #endregion

        #region ChangeLevel

        public ActionResult ChangeLevel(string id)
        {
            CustomerLevelDetailView customerLevelDetailView = new CustomerLevelDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ChangeLevel");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerLevelDetailView);
            }
            #endregion

            customerLevelDetailView.CustomerView = this.GetCustomerView(id);
            customerLevelDetailView.EmployeeView = GetEmployee();
            customerLevelDetailView.QueryViews = _queryService.GetQueries(new GetQueriesRequest() { EmployeeID = GetEmployee().ID }).QueryViews;

            #region DropDowns
            AjaxGetRequest getLevelReq = new AjaxGetRequest() { ID = customerLevelDetailView.CustomerView.LevelID };
            if (getLevelReq.ID != Guid.Empty)
            {
                #region DropDownList For Level

                IEnumerable<LevelView> nextLevelViews = _levelService.GetNextLevels(getLevelReq).data;
                List<DropDownItem> list = new List<DropDownItem>();
                list.Add(null);

                if (nextLevelViews != null)
                    foreach (LevelView nextLevelView in nextLevelViews)
                    {
                        list.Add(new DropDownItem { Value = nextLevelView.ID.ToString(), Text = nextLevelView.LevelTitle });
                    }
                var selectList = new SelectList(list, "Value", "Text");
                ViewData["Levels"] = selectList;

                #endregion
            }
            else
            {
                #region DropDownList For LevelType

                customerLevelDetailView.LevelTypeViews = _levelTypeService.GetLevelTypes().LevelTypeViews;
                List<DropDownItem> tList = new List<DropDownItem>();
                tList.Add(null);

                if (customerLevelDetailView.LevelTypeViews != null)
                    foreach (LevelTypeView levelTypeView in customerLevelDetailView.LevelTypeViews)
                    {
                        tList.Add(new DropDownItem { Value = levelTypeView.ID.ToString(), Text = levelTypeView.Title });
                    }
                var selectList = new SelectList(tList, "Value", "Text");
                ViewData["LevelTypes"] = selectList;

                #endregion

                #region DropDownList For Level

                List<DropDownItem> Tlist = new List<DropDownItem>();
                Tlist.Add(new DropDownItem { Value = "", Text = "-- نوع چرخه را انتخاب کنید --" });

                var TselectList = new SelectList(Tlist, "Value", "Text");
                ViewData["Levels"] = TselectList;

                #endregion

            }
            #endregion

            return View(customerLevelDetailView);
        }

        [HttpPost]
        public ActionResult ChangeLevel(string id, CustomerLevelDetailView customerLevelDetailView)
        {
            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_ChangeLevel");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerLevelDetailView);
            }
            #endregion

            customerLevelDetailView.CustomerView = GetCustomerView(id);
            customerLevelDetailView.EmployeeView = GetEmployee();
            customerLevelDetailView.QueryViews = _queryService.GetQueries(new GetQueriesRequest() { EmployeeID = GetEmployee().ID }).QueryViews;

            #region DropDowns
            AjaxGetRequest getLevelReq = new AjaxGetRequest() { ID = customerLevelDetailView.CustomerView.LevelID };
            if (getLevelReq.ID != Guid.Empty)
            {
                #region DropDownList For Level

                IEnumerable<LevelView> nextLevelViews = _levelService.GetNextLevels(getLevelReq).data;
                List<DropDownItem> list = new List<DropDownItem>();
                list.Add(null);

                if (nextLevelViews != null)
                    foreach (LevelView nextLevelView in nextLevelViews)
                    {
                        list.Add(new DropDownItem { Value = nextLevelView.ID.ToString(), Text = nextLevelView.LevelTitle });
                    }
                var selectList = new SelectList(list, "Value", "Text");
                ViewData["Levels"] = selectList;

                #endregion
            }
            else
            {
                #region DropDownList For LevelType

                customerLevelDetailView.LevelTypeViews = _levelTypeService.GetLevelTypes().LevelTypeViews;
                List<DropDownItem> tList = new List<DropDownItem>();
                tList.Add(null);

                if (customerLevelDetailView.LevelTypeViews != null)
                    foreach (LevelTypeView levelTypeView in customerLevelDetailView.LevelTypeViews)
                    {
                        tList.Add(new DropDownItem { Value = levelTypeView.ID.ToString(), Text = levelTypeView.Title });
                    }
                var selectList = new SelectList(tList, "Value", "Text");
                ViewData["LevelTypes"] = selectList;

                #endregion

                #region DropDownList For Level

                List<DropDownItem> Tlist = new List<DropDownItem>();
                Tlist.Add(new DropDownItem { Value = "", Text = "-- نوع چرخه را انتخاب کنید --" });

                var TselectList = new SelectList(Tlist, "Value", "Text");
                ViewData["Levels"] = TselectList;

                #endregion

            }
            #endregion

            AddCustomerLevelRequest request = new AddCustomerLevelRequest()
            {
                NewLevelID = customerLevelDetailView.CustomerLevelView.LevelID,
                CustomerID = Guid.Parse(id),
                CreateEmployeeID = GetEmployee().ID,
                Note = customerLevelDetailView.CustomerLevelView.Note
            };

            if (request.NewLevelID == Guid.Empty)
            {
                ModelState.AddModelError("", "چرخه و مرحله مشتری باید وارد شود.");
                return View(customerLevelDetailView);
            }

            GeneralResponse response = _customerLevelService.AddCustomerLevel(request);

            if (response.success)
            {
                TempData["CustomerID"] = request.CustomerID;

                return RedirectToAction("ChangeLevelsuccess");
            }
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(customerLevelDetailView);
            }
        }

        public ActionResult ChangeLevelsuccess()
        {
            CustomerDetailView customerDetailView = new CustomerDetailView();
            customerDetailView.EmployeeView = GetEmployee();
            customerDetailView.QueryViews = _queryService.GetQueries(new GetQueriesRequest() { EmployeeID = GetEmployee().ID }).QueryViews;

            // بدست آوردن مشخصات مشتری
            if (TempData["CustomerID"] != null)
            {
                string id = TempData["CustomerID"].ToString();

                customerDetailView.CustomerView = GetCustomerView(id);
            }
            else
            {
                // در این صورت احتمالا کاربر بصورت دستی یو آر ال وارد کرده است
                customerDetailView.CustomerView = null;
            }

            return View(customerDetailView);
        }

        #endregion

        #region Queries

        public ActionResult Queries()
        {
            QueryDetailView queryDetailView = new QueryDetailView();
            queryDetailView.EmployeeView = GetEmployee();
            queryDetailView.QueryViews = _queryService.GetQueries(new GetQueriesRequest() { EmployeeID = GetEmployee().ID }).QueryViews;

            return View(queryDetailView);
        }

        #endregion

        #region Query

        public ActionResult Query(string id)
        {
            CustomerHomePageView customerHomePageView = new CustomerHomePageView();
            customerHomePageView.EmployeeView = GetEmployee();

            #region Get QueryInfo

            GetRequest getRequest = new GetRequest() { ID = Guid.Parse(id) };
            customerHomePageView.QueryView = _queryService.GetQuery(getRequest).QueryView;
            customerHomePageView.QueryViews = _queryService.GetQueries(new GetQueriesRequest() { EmployeeID = GetEmployee().ID }).QueryViews;
            Guid queryID = customerHomePageView.QueryView.ID;

            #endregion

            QuickSearchRequest getReq = new QuickSearchRequest();
            getReq.pageSize = 10;
            getReq.pageNumber = 1;
            getReq.queryID = queryID;
            GetGeneralResponse<IEnumerable<CustomerView>> customerResponse = this._customerService.FindCustomers(getReq, GetEmployee().ID);

            customerHomePageView.CustomerViews = customerResponse.data;
            customerHomePageView.Count = customerResponse.totalCount;

            DataSourceRequest request = new DataSourceRequest
            {
                PageSize = 10,
                Page = 1
            };

            CustomerQuery_Read(id, request);

            return View(customerHomePageView);
        }

        public ActionResult CustomerQuery_Read(string id, [DataSourceRequest] DataSourceRequest request)
        {
            CustomerHomePageView customerHomePageView = new CustomerHomePageView();
            customerHomePageView.EmployeeView = GetEmployee();

            #region Get Query Info
            GetRequest getRequest = new GetRequest() { ID = Guid.Parse(id) };
            customerHomePageView.QueryView = _queryService.GetQuery(getRequest).QueryView;
            customerHomePageView.QueryViews = _queryService.GetQueries(new GetQueriesRequest() { EmployeeID = GetEmployee().ID }).QueryViews;
            Guid queryID = customerHomePageView.QueryView.ID;
            #endregion

            QuickSearchRequest getReq = new QuickSearchRequest();
            getReq.pageSize = request.PageSize == 0 ? 10 : request.PageSize;
            getReq.pageNumber = request.Page;
            getReq.queryID = queryID;
            GetGeneralResponse<IEnumerable<CustomerView>> customerResponse = this._customerService.FindCustomers(getReq, GetEmployee().ID);

            customerHomePageView.CustomerViews = customerResponse.data;
            customerHomePageView.Count = customerResponse.totalCount;


            var result = new DataSourceResult()
            {
                Data = customerResponse.data,
                Total = customerResponse.totalCount
            };
            return Json(result);
        }

        #endregion

        #endregion

        #region Simple Employee

        public JsonResult SimpleCustomer_ReadADO(string ADSLPhone)
        {
            var con = ConfigurationManager.ConnectionStrings["SamanCnn"].ToString();

            SimpleCustomerView simpleEmployee = new SimpleCustomerView();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string oString =
                    "select t1.CustomerID,t1.FirstName,t1.LastName,t2.LevelTitle from cus.Customer t1 inner join cus.Level t2 on t1.LevelID=t2.LevelID where t1.ADSLPhone=@ADSLPhone";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                oCmd.Parameters.AddWithValue("@ADSLPhone", ADSLPhone);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        simpleEmployee.CustomerID = Guid.Parse(oReader["CustomerID"].ToString());
                        simpleEmployee.FirstName = oReader["FirstName"].ToString();
                        simpleEmployee.Lastname = oReader["LastName"].ToString();
                        simpleEmployee.LevelTitle = oReader["LevelTitle"].ToString();
                    }

                    myConnection.Close();
                }
            }

            return Json(simpleEmployee, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SimpleCustomer_Read(string ADSLPhone)
        {
            GetGeneralResponse<SimpleCustomerView> response = new GetGeneralResponse<SimpleCustomerView>();

            response = _customerService.GetSimpleCustomer(ADSLPhone);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public SimpleCustomerView SimpleCustomer_Read_Model(string ADSLPhone)
        {
            GetGeneralResponse<SimpleCustomerView> response = new GetGeneralResponse<SimpleCustomerView>();

            response = _customerService.GetSimpleCustomer(ADSLPhone);

            return response.data;
        }

        #endregion

        #region Customers CRUD

        public JsonResult Customers_Read(Guid queryID, int? pageSize, int? pageNumber, string query, string sort,IList<FilterData> filter,Guid? customerID)
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();

            EmployeeView employee = GetEmployee();

            #region Access Check
            bool hasPermission = employee.IsGuaranteed("Customer_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion
            Guid CustomerID = customerID == null ? Guid.Empty : (Guid)customerID;
            if (CustomerID != Guid.Empty)
            {
                GetGeneralResponse<CustomerView> _res = new GetGeneralResponse<CustomerView>();
                GetCustomerResponse cus =  _customerService.GetCustomer(new GetRequest(){ID=CustomerID});
                IList<CustomerView> customer =new List<CustomerView>();
                CustomerView c = cus.CustomerView;
                customer.Add(c);
                response.data = customer;
                return Json(response,JsonRequestBehavior.AllowGet);
            }
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            IList<Sort> Sorts = ConvertJsonToObject(sort);

            foreach (Sort item in Sorts)
            {
                if (item.SortColumn == "Name")
                    item.SortColumn = "LastName";
            }

            if (query != null)
            {
                QuickSearchRequest request = new QuickSearchRequest()
                {
                    pageNumber = PageNumber,
                    pageSize = PageSize,
                    query = query,
                    queryID = queryID,
                    sort=Sorts
                };

                response = _customerService.FindCustomers(request,employee.ID); ;
            }

            else if (customerID != null)
            {
                QuickSearchRequest request = new QuickSearchRequest()
                {
                    pageNumber = PageNumber,
                    pageSize = PageSize,
                    query = query,
                    queryID = queryID,
                    sort = Sorts,
                    customerID=customerID
                };

                response = _customerService.FindCustomers(request, employee.ID); ;
            }
            else
            {
                response = this._customerService.GetCustomers(queryID, employee.ID, PageSize, PageNumber, Sorts, filter);
            }

            if (queryID == Guid.Parse("12d942e9-9b2f-42a9-82d5-66d661fac17b") && query != string.Empty )
            {
                IList<CustomerView> customer = new List<CustomerView>();
                customer.Add(response.data.FirstOrDefault());
                response.data = customer;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else if (queryID == Guid.Parse("12d942e9-9b2f-42a9-82d5-66d661fac17b") && query == string.Empty )
            {
                IList<CustomerView> customer = new List<CustomerView>();
                customer.Add(response.data.FirstOrDefault());
                response.data = null;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Customer_Update(
            Guid CustomerID, string Gender, string FirstName, string LastName, Guid BuyPossibilityID, Guid FollowStatusID,
            Guid? AgencyID, string BirthDate, string Email, string LegalType, string Job, string Mobile2, Guid? NetworkID, 
            string Phone, string SFirstName, string SLastName, Guid SuctionModeID,Guid SuctionModeDetailID,string Address, string Note,
            bool Discontinued, int RowVersion, string Mobile1, Guid DocumentStatusID)
        {
            GeneralResponse response=new GeneralResponse();
            
            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            EditCustomerRequest request = new EditCustomerRequest()
            {
                ID = CustomerID,
                AgencyID = AgencyID == null ? Guid.Empty : (Guid)AgencyID,
                BirthDate = BirthDate,
                Email = Email,
                Gender = Gender,
                FirstName = FirstName,
                LastName = LastName,
                BuyPossibilityID = BuyPossibilityID,
                FollowStatusID = FollowStatusID,
                LegalType = LegalType,
                Job = Job,
                Mobile1 = Mobile1,
                Mobile2 = Mobile2,
                NetworkID = NetworkID == null ? Guid.Empty : (Guid)NetworkID,
                Phone = Phone,
                SFirstName = SFirstName,
                SLastName = SLastName,
                SuctionModeID = SuctionModeID,
                SuctionModeDetailID=SuctionModeDetailID,
                Address = Address,
                Note = Note,
                Discontinued = Discontinued,
                DocumentStatusID = DocumentStatusID,
                RowVersion = RowVersion
            };
            request.ModifiedEmployeeID = GetEmployee().ID;

            response = _customerService.EditCustomer(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult testMethod()
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();

            IList<FilterModel> filter = new List<FilterModel>();
            filter.Add(new FilterModel() {
                field = "ADSLPhone",
                type="list",
                value=new[]{"44001010","44003030"},
            });
            filter.Add(new FilterModel()
            {
                field = "Balance",
                type = "numeric",
                comparsion = "lt",
                value = new[] { "100000000" },
            });
            filter.Add(new FilterModel()
            {
                field = "Balance",
                type = "numeric",
                comparsion = "gt",
                value = new[] { "0" },
            });
            filter.Add(new FilterModel()
            {
                field = "Balance",
                type = "numeric",
                comparsion = "gt",
                value = new[] { "0" },
            });
            filter.Add(new FilterModel() {
                field = "CenterName",
                type="list",
                value = new[] { "3981b556-4454-4c0a-842c-2069051fc964" },
            });
            //filter.Add(new FilterModel()
            //{
            //    field = "FirstName",
            //    type = "string",
            //    value = new[] { "محمد" },
            //});

            //response = _customerService.test(filter);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Customer_Insert(
            string Gender, string FirstName, string LastName, string Mobile1, Guid BuyPossibilityID, Guid FollowStatusID,
            Guid? AgencyID, string BirthDate, string Email, string LegalType, string Job, string Mobile2, Guid? NetworkID, 
            string Phone, string SFirstName, string SLastName, Guid SuctionModeID,Guid SuctionModeDetailID, string Address, string Note,
            bool Discontinued, Guid LevelID, string AdslPhone, Guid LevelTypeID)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            AddCustomerRequest request = new AddCustomerRequest()
            {
                CreateEmployeeID =GetEmployee().ID,
                FirstName = FirstName,
                LastName = LastName,
                BuyPossibilityID = BuyPossibilityID,
                FollowStatusID = FollowStatusID,
                Gender = Gender,
                ADSLPhone = AdslPhone,
                Mobile1 = Mobile1,
                LevelID = LevelID,
                levelTypeID=LevelTypeID,
                AgencyID = AgencyID == null ? Guid.Empty : (Guid)AgencyID,
                BirthDate = BirthDate,
                Email = Email,
                LegalType = LegalType,
                Job = Job,
                Mobile2 = Mobile2,
                NetworkID = NetworkID == null ? Guid.Empty : (Guid) NetworkID,
                Phone = Phone,
                SFirstName = SFirstName,
                SLastName = SLastName,
                SuctionModeID = SuctionModeID,
                SuctionModeDetailID=SuctionModeDetailID,
                Address = Address,
                Note = Note,
                Discontinued = Discontinued,
                Balance=0
                
           };

            response = _customerService.QuickAddCustomer(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Customer_Delete(Guid customerID)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            DeleteRequest request = new DeleteRequest()
            {
                ID = customerID
            };

            response = _customerService.DeleteCustomer(request);

            

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Customer_Read_ByID(Guid customerID)
        {
            GetCustomerResponse response = new GetCustomerResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion


            GetRequest request = new GetRequest()
            {
                ID = customerID
            };
            
            response = _customerService.GetCustomer(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public JsonResult LevelHistory(Guid customerID, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<CustomerLevelView>> response =new GetGeneralResponse<IEnumerable<CustomerLevelView>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _customerLevelService.GetLevelHistory(customerID, PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);

        }


        #region مشتریانی که حداقل یک صندوق دارند و در واحد ثبت نام هستند و بید به واحدارسل برای رانژه رسال شونند

        public JsonResult CustomerMustoGoToRanje_Read(int? pageSize,int?pageNumber,IList<FilterData> filter,string sort)
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response=new GetGeneralResponse<IEnumerable<CustomerView>>();



            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _customerService.GetCustomerMustoGoToRanje(PageSize, PageNumber,filter,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Notes

        public JsonResult Notes_Read(Guid customerID, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<NoteView>> response=new GetGeneralResponse<IEnumerable<NoteView>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Note_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _noteService.GetNotes(customerID, PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Note_Insert(Guid customerID, string NoteDescription)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Note_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            AddNoteRequest request = new AddNoteRequest();
            request.CreateEmployeeID = GetEmployee().ID;
            request.CustomerID = customerID;
            request.NoteDescription = NoteDescription;

            response = this._noteService.AddNote(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Note_Delete(Guid noteID)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Note_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = new GeneralResponse();

            response = _noteService.DeleteNote(new DeleteRequest() { ID = noteID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Note_Edit(Guid noteID, string NoteDescription, int rowVersion)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Note_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            EditNoteRequest request = new EditNoteRequest();
            request.ID = noteID;
            request.RowVersion = rowVersion;
            request.ModifiedEmployeeID = GetEmployee().ID;
            request.NoteDescription = NoteDescription;

            response = _noteService.EditNote(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region Send Bradcast SMS And Email
        [ValidateInput(false)]
        public JsonResult Customers_SendEmailAndSMS(IEnumerable<Guid> IDs, string subject, string Content,
            string Message)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_SendEmailAndsms");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _customerService.SendEmalAndSms(IDs, subject, Content, Message, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Change Level

        public JsonResult Change_Level(Guid customerID, Guid newLevelID, string note)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Change_Level");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            AddCustomerLevelRequest request = new AddCustomerLevelRequest()
            {
                CustomerID = customerID,
                NewLevelID = newLevelID,
                Note = note,
                CreateEmployeeID = GetEmployee().ID
            };

            response = _customerLevelService.AddCustomerLevel(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Change_Level_NoPermission(Guid customerID, Guid newLevelID, string note)
        {
            GeneralResponse response = new GeneralResponse();

            AddCustomerLevelRequest request = new AddCustomerLevelRequest()
            {
                CustomerID = customerID,
                NewLevelID = newLevelID,
                Note = note,
                CreateEmployeeID = GetSimpleEmployee().ID
            };

            response = _customerLevelService.AddCustomerLevel(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region احتمال خرید و وضعیت پیگیری

        public JsonResult BuyPossibilities_Read()
        {
            GetGeneralResponse<IEnumerable<BuyPossibilityView>> response=new GetGeneralResponse<IEnumerable<BuyPossibilityView>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _buyPossibilityService.GetBuyPossibilities();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuyPossibilities_Read_NoPermission()
        {
            GetGeneralResponse<IEnumerable<BuyPossibilityView>> response = new GetGeneralResponse<IEnumerable<BuyPossibilityView>>();

            response = _buyPossibilityService.GetBuyPossibilities();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FollowStatuss_Read()
        {
            GetGeneralResponse<IEnumerable<FollowStatusView>> response = _followStatusService.GetFollowStatuss();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CenterInfo

        public JsonResult CenterInfo(string adslPhone)
        {
            GetCenterInfoResponse centerInfo = new GetCenterInfoResponse();


            centerInfo = _centerService.GetCenterInfo(adslPhone, 5);
            
            var result = new
            {
                CenterId=centerInfo.CenterID,
                success = true,
                hasCenter = centerInfo.hasCenter,
                centerInfo = String.Format("مرکز مخابراتی: {0} - {1}: {2}", centerInfo.CenterName, "وضعیت پشتیبانی", centerInfo.Status),
                note = centerInfo.Center.Note

            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Members

        private CustomerView GetCustomerView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetCustomerResponse response = this._customerService.GetCustomer(request);

            return response.CustomerView;
        }
        #endregion

        #region Reporting
        #endregion
        
        #region Send SMS

        public JsonResult SendSMS(IEnumerable<Guid> IDs,string Message)
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();


            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Customer_SendSms");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _customerService.SendSMS(IDs, Message,GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Send Email
        [ValidateInput(false)]
        public JsonResult SendEmail(IEnumerable<Guid> IDs, string Content, string Subject)
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Sms_Send");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion


            response = _customerService.SendEmail(IDs, Content, Subject, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Customer Read By ID

        public JsonResult GetCustomer_ByID(Guid CustomerID)
        {
            GetGeneralResponse<CustomerView> response = new GetGeneralResponse<CustomerView>();

            response.data = _customerService.GetCustomerByID(CustomerID);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region CustomersReport

        public bool ValidateADSLPhone(string ADSLPhone)
        {
            CustomerView customer = new CustomerView();
            customer = _customerService.getCustomerbyPhone(ADSLPhone);

            if (customer != null)
                return true;
            else
                return false;
        }


        #endregion

        #region VOIP Search

        #endregion

        public JsonResult GetLevelOptions(Guid customerID)
        {
            GetGeneralResponse<LevelOptionsView> response = this._customerService.GetLevelOptions(customerID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LazyTest()
        {
            string response = _customerService.LazyTest("From Customer");

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult sendTestEmail()
        //{
        //    List<Guid> Ids = new List<Guid>();
        //    Ids.Add(Guid.Parse("33CD02F5-6751-4440-8BF1-46D196B8722F"));

        //    return Json(_customerService.SendEmail(Ids, "مشتری گرامی <%Title%> <%Name%> شما توسط <%SaleEmployeeName%> ثبت نام شدید و با تلفن <%ADSLPhone%> وارد مرحله <%Level%> شدید. باقیمانده اعتبار شما <%Balance%> می باشد.", "موضوع", GetEmployee().ID), JsonRequestBehavior.AllowGet);
        //}
    }
}
