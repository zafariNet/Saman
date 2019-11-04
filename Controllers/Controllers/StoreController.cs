#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controllers.ViewModels.Reports;
using Services.Interfaces;
using Controllers.ViewModels.StoreCatalog;
using System.Web.Mvc;
using Services.ViewModels.Store;
using Services.Messaging;
using Services.Messaging.StoreCatalogService;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Controllers.ViewModels;
using Services.ViewModels.Employees;


#endregion

namespace Controllers.Controllers
{   
    [Authorize]
    public class StoreController: BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly IStoreService _storeService;


        #endregion

        #region Ctor
        public StoreController(IEmployeeService employeeService, IStoreService storeService)
            : base(employeeService)
        {
            this._storeService = storeService;
            this._employeeService = employeeService;
        }
        #endregion

        #region Old Methods

        #region Ajax Read
        public ActionResult Store_Read([DataSourceRequest] DataSourceRequest request)
        {

            GetStoresResponse storeResponse =new GetStoresResponse();
            StoreHomePageView storeHomePageView = new StoreHomePageView();
            storeHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Store_Read");
            if (!hasPermission)
            {

                var _result = new DataSourceResult()
                {
                    Data = storeResponse.StoreViews,
                    Total = storeResponse.Count
                };
                ModelState.AddModelError("", "AccessDenied");
                return Json(_result);
            }
            #endregion

            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            getRequest.PageNumber = request.Page;
            storeResponse = this._storeService.GetStores(getRequest);

            storeHomePageView.StoreViews = storeResponse.StoreViews;
            storeHomePageView.Count = storeResponse.Count;

            var result = new DataSourceResult()
            {
                Data = storeResponse.StoreViews,
                Total = storeResponse.Count
            };
            return Json(result);
        }
        #endregion

        #region Insert
        public ActionResult Index()
        {
            StoreHomePageView storeHomePageView = new StoreHomePageView();
            storeHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Store_Read");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(storeHomePageView);
            }
            #endregion

            storeHomePageView.StoreViews = this._storeService.GetStores().StoreViews;

            return View(storeHomePageView);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            StoreDetailView storeDetailView = new StoreDetailView();
            storeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Store_Insert");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(storeDetailView);
            }
            #endregion
            
            #region DropDownList For Owner Employee
            
            storeDetailView.EmployeeViews = _employeeService.GetInstallExprets().EmployeeViews;
            List<DropDownItem> list = new List<DropDownItem>();
            list.Add(null);

            if (storeDetailView.EmployeeViews != null)
                foreach (EmployeeView employeeView in storeDetailView.EmployeeViews)
                {
                    list.Add(new DropDownItem { Value = employeeView.ID.ToString(), Text = employeeView.Name });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["InstallExprets"] = selectList;
            #endregion

            return View(storeDetailView);
        }

        [HttpPost]
        public ActionResult Create(StoreDetailView storeDetailView)
        {
            storeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Store_Insert");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(storeDetailView);
            }
            #endregion

            #region DropDownList For Owner Employee

            storeDetailView.EmployeeViews = _employeeService.GetInstallExprets().EmployeeViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (storeDetailView.EmployeeViews != null)
                foreach (EmployeeView employeeView in storeDetailView.EmployeeViews)
                {
                    list.Add(new DropDownItem { Value = employeeView.ID.ToString(), Text = employeeView.Name });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["InstallExprets"] = selectList;
            #endregion

            
            if (ModelState.IsValid)
                try
                {
                    AddStoreRequestOld request = new AddStoreRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.OwnerEmployeeID = storeDetailView.StoreView.OwnerEmployeeID;
                    request.StoreName = storeDetailView.StoreView.StoreName;
                    request.Note = storeDetailView.StoreView.Note;

                    GeneralResponse response = this._storeService.AddStore(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(storeDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(storeDetailView);
                }

            return View(storeDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            StoreDetailView storeDetailView = new StoreDetailView();
            storeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Store_Update");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(storeDetailView);
            }
            #endregion

            storeDetailView.StoreView = this.GetStoreView(id);
            
            #region DropDownList For Owner Employee

            storeDetailView.EmployeeViews = _employeeService.GetInstallExprets().EmployeeViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (storeDetailView.EmployeeViews != null)
                foreach (EmployeeView employeeView in storeDetailView.EmployeeViews)
                {
                    list.Add(new DropDownItem { Value = employeeView.ID.ToString(), Text = employeeView.Name });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["InstallExprets"] = selectList;
            #endregion

            return View(storeDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, StoreDetailView storeDetailView)
        {
            storeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Store_Update");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(storeDetailView);
            }
            #endregion


            #region DropDownList For Owner Employee

            storeDetailView.EmployeeViews = _employeeService.GetInstallExprets().EmployeeViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (storeDetailView.EmployeeViews != null)
                foreach (EmployeeView employeeView in storeDetailView.EmployeeViews)
                {
                    list.Add(new DropDownItem { Value = employeeView.ID.ToString(), Text = employeeView.Name });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["InstallExprets"] = selectList;
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditStoreRequestOld request = new EditStoreRequestOld();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.OwnerEmployeeID = storeDetailView.StoreView.OwnerEmployeeID;
                    request.StoreName = storeDetailView.StoreView.StoreName;
                    request.Note = storeDetailView.StoreView.Note;
                    request.RowVersion = storeDetailView.StoreView.RowVersion;

                    GeneralResponse response = this._storeService.EditStore(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(storeDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(storeDetailView);
                }

            return View(storeDetailView);
        }
        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            StoreDetailView storeDetailView = new StoreDetailView();
            storeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Store_Read");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(storeDetailView);
            }
            #endregion

            StoreView storeView = this.GetStoreView(id);
            
            storeDetailView.StoreView = storeView;
            
            return View(storeDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            StoreDetailView storeDetailView = new StoreDetailView();
            storeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Store_Delete");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(storeDetailView);
            }
            #endregion

            storeDetailView.StoreView = this.GetStoreView(id);
            
            return View(storeDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            StoreDetailView storeDetailView = new StoreDetailView();
            storeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Store_Delete");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(storeDetailView);
            }
            #endregion

            storeDetailView.StoreView = this.GetStoreView(id);
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._storeService.DeleteStore(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(storeDetailView);
            }
        }
        #endregion

        #endregion

        #region New Methods

        #region Read

        public JsonResult Stores_Read(int? pageSize, int? pageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<StoreView>> response = new GetGeneralResponse<IEnumerable<StoreView>>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Store_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion
            
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _storeService.GetStores(PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Inert

        public JsonResult Stores_Insert(IEnumerable<AddStoreRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Store_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _storeService.AddStores(requests, GetEmployee().ID);

            return Json(response,JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult Stores_Update(IEnumerable<EditStoreRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Store_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _storeService.EditStores(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Delete

        public JsonResult Stores_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Store_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _storeService.DeleteStores(requests);

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #endregion

        #region Private Members

        private StoreView GetStoreView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetStoreResponse response = this._storeService.GetStore(request);

            return response.StoreView;
        }

        #endregion

    }
}
