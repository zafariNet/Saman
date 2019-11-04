using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.StoreCatalog;
using System.Web.Mvc;
using Services.ViewModels.Store;
using Services.Messaging;
using Services.Messaging.StoreCatalogService;
using Controllers.ViewModels;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Infrastructure.Querying;

namespace Controllers.Controllers
{
    [Authorize]
    public class CreditServiceController: BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly ICreditServiceService _creditServiceService;

        private readonly INetworkService _networkService;
        #endregion

        #region Ctor
        public CreditServiceController(IEmployeeService employeeService, ICreditServiceService creditServiceService
            , INetworkService networkService)
            : base(employeeService)
        {
            this._creditServiceService = creditServiceService;
            this._employeeService = employeeService;
            _networkService = networkService;
        }
        #endregion

        #region Old Methods

        #region Ajax Read
        public ActionResult UncreditService_Read([DataSourceRequest] DataSourceRequest request)
        {
            CreditServiceHomePageView creditServiceHomePageView = new CreditServiceHomePageView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("UncreditService_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(creditServiceHomePageView.CreditServiceViews.ToDataSourceResult(request));
            }
            #endregion

            creditServiceHomePageView.EmployeeView = GetEmployee();
            creditServiceHomePageView.CreditServiceViews = this._creditServiceService.GetCreditServices().CreditServiceViews;

            return Json(creditServiceHomePageView.CreditServiceViews.ToDataSourceResult(request));
        }
        #endregion

        #region Index
        public ActionResult Index()
        {
            CreditServiceHomePageView creditServiceHomePageView = new CreditServiceHomePageView();
            creditServiceHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CreditService_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(creditServiceHomePageView);
            }
            #endregion
            
            creditServiceHomePageView.CreditServiceViews = this._creditServiceService.GetCreditServices().CreditServiceViews;

            return View(creditServiceHomePageView);
        }
        #endregion

        #region Moving Up and Down
        
        public ActionResult MoveUp(string id)
        {
            MoveRequest request = new MoveRequest();
            request.ID = Guid.Parse(id);

            MoveResponse response = _creditServiceService.MoveUp(request);

            return RedirectToAction("Index");
        }

        public ActionResult MoveDown(string id)
        {
            MoveRequest request = new MoveRequest();
            request.ID = Guid.Parse(id);

            MoveResponse response = _creditServiceService.MoveDown(request);

            return RedirectToAction("Index");
        }

        #endregion

        #region Create
        public ActionResult Create()
        {
            CreditServiceDetailView creditServiceDetailView = new CreditServiceDetailView();
            creditServiceDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CreditService_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(creditServiceDetailView);
            }
            #endregion
            
            #region DropDownList For Network
            creditServiceDetailView.NetworkViews = _networkService.GetNetworks().NetworkViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (creditServiceDetailView.NetworkViews != null)
                foreach (NetworkView networkView in creditServiceDetailView.NetworkViews)
                {
                    list.Add(new DropDownItem { Value = networkView.ID.ToString(), Text = networkView.NetworkName });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["Networks"] = selectList;
            #endregion

            return View(creditServiceDetailView);
        }

        [HttpPost]
        public ActionResult Create(CreditServiceDetailView creditServiceDetailView)
        {
            creditServiceDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CreditService_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(creditServiceDetailView);
            }
            #endregion

            #region DropDownList For Network
            creditServiceDetailView.NetworkViews = _networkService.GetNetworks().NetworkViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (creditServiceDetailView.NetworkViews != null)
                foreach (NetworkView networkView in creditServiceDetailView.NetworkViews)
                {
                    list.Add(new DropDownItem { Value = networkView.ID.ToString(), Text = networkView.NetworkName });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["Networks"] = selectList;
            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddCreditServiceRequestOld request = new AddCreditServiceRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.CreditServiceCode = creditServiceDetailView.CreditServiceView.CreditServiceCode;
                    request.Discontinued = creditServiceDetailView.CreditServiceView.Discontinued;
                    request.ExpDays = creditServiceDetailView.CreditServiceView.ExpDays;
                    request.Imposition = creditServiceDetailView.CreditServiceView.Imposition;
                    request.MaxDiscount = Convert.ToInt64(creditServiceDetailView.CreditServiceView.MaxDiscount);
                    request.Note = creditServiceDetailView.CreditServiceView.Note;
                    request.NetworkID = creditServiceDetailView.CreditServiceView.NetworkID;
                    request.PurchaseUnitPrice = Convert.ToInt64(creditServiceDetailView.CreditServiceView.PurchaseUnitPrice);
                    request.ResellerUnitPrice = Convert.ToInt64(creditServiceDetailView.CreditServiceView.ResellerUnitPrice);
                    request.ServiceName = creditServiceDetailView.CreditServiceView.ServiceName;
                    request.UnitPrice = Convert.ToInt64(creditServiceDetailView.CreditServiceView.UnitPrice);

                    GeneralResponse response = this._creditServiceService.AddCreditService(request);

                    if (!response.hasError)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(creditServiceDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(creditServiceDetailView);
                }

            return View(creditServiceDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            CreditServiceDetailView creditServiceDetailView = new CreditServiceDetailView();
            creditServiceDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CreditService_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(creditServiceDetailView);
            }
            #endregion

            creditServiceDetailView.CreditServiceView = this.GetCreditServiceView(id);
            
            #region DropDownList For Network
            creditServiceDetailView.NetworkViews = _networkService.GetNetworks().NetworkViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (creditServiceDetailView.NetworkViews != null)
                foreach (NetworkView networkView in creditServiceDetailView.NetworkViews)
                {
                    list.Add(new DropDownItem { Value = networkView.ID.ToString(), Text = networkView.NetworkName });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["Networks"] = selectList;
            #endregion

            return View(creditServiceDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, CreditServiceDetailView creditServiceDetailView)
        {
            creditServiceDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CreditService_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(creditServiceDetailView);
            }
            #endregion

            #region DropDownList For Network
            creditServiceDetailView.NetworkViews = _networkService.GetNetworks().NetworkViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (creditServiceDetailView.NetworkViews != null)
                foreach (NetworkView networkView in creditServiceDetailView.NetworkViews)
                {
                    list.Add(new DropDownItem { Value = networkView.ID.ToString(), Text = networkView.NetworkName });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["Networks"] = selectList;
            #endregion

            
            if (ModelState.IsValid)
                try
                {
                    EditCreditServiceRequestOld request = new EditCreditServiceRequestOld();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.CreditServiceCode = creditServiceDetailView.CreditServiceView.CreditServiceCode;
                    request.Discontinued = creditServiceDetailView.CreditServiceView.Discontinued;
                    request.ExpDays = creditServiceDetailView.CreditServiceView.ExpDays;
                    request.Imposition = creditServiceDetailView.CreditServiceView.Imposition;
                    request.MaxDiscount = Convert.ToInt64(creditServiceDetailView.CreditServiceView.MaxDiscount);
                    request.Note = creditServiceDetailView.CreditServiceView.Note;
                    request.NetworkID = creditServiceDetailView.CreditServiceView.NetworkID;
                    request.PurchaseUnitPrice = Convert.ToInt64(creditServiceDetailView.CreditServiceView.PurchaseUnitPrice);
                    request.ResellerUnitPrice = Convert.ToInt64(creditServiceDetailView.CreditServiceView.ResellerUnitPrice);
                    request.ServiceName = creditServiceDetailView.CreditServiceView.ServiceName;
                    request.UnitPrice = Convert.ToInt64(creditServiceDetailView.CreditServiceView.UnitPrice);
                    request.RowVersion = creditServiceDetailView.CreditServiceView.RowVersion;

                    GeneralResponse response = this._creditServiceService.EditCreditService(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(creditServiceDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(creditServiceDetailView);
                }

            return View(creditServiceDetailView);
        }
        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            CreditServiceDetailView creditServiceDetailView = new CreditServiceDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CreditService_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(creditServiceDetailView);
            }
            #endregion

            CreditServiceView creditServiceView = this.GetCreditServiceView(id);

            creditServiceDetailView.CreditServiceView = creditServiceView;
            creditServiceDetailView.EmployeeView = GetEmployee();

            return View(creditServiceDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            CreditServiceDetailView creditServiceDetailView = new CreditServiceDetailView();
            creditServiceDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CreditService_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(creditServiceDetailView);
            }
            #endregion

            creditServiceDetailView.CreditServiceView = this.GetCreditServiceView(id);
            
            return View(creditServiceDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            CreditServiceDetailView creditServiceDetailView = new CreditServiceDetailView();
            creditServiceDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CreditService_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(creditServiceDetailView);
            }
            #endregion

            creditServiceDetailView.CreditServiceView = this.GetCreditServiceView(id);
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._creditServiceService.DeleteCreditService(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(creditServiceDetailView);
            }
        }
        #endregion

        #endregion

        #region new Methods

        #region Read

        public JsonResult CreditServices_Read(int? pageSize, int? pageNumber,string sort,IList<FilterData> filter)
        {
            GetGeneralResponse<IEnumerable<CreditServiceView>> response = new GetGeneralResponse<IEnumerable<CreditServiceView>>();

            //#region Access Check
            //bool hasPermission = GetEmployee().IsGuaranteed("CreditService_Read");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
            //#endregion

            
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _creditServiceService.GetCreditServices(PageSize, PageNumber,ConvertJsonToObject(sort),filter);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public JsonResult CreditServices_Reads(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<CreditServiceView>> response = new GetGeneralResponse<IEnumerable<CreditServiceView>>();


            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _creditServiceService.GetCreditServices(PageSize, PageNumber);
            
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        
        #endregion

        #region Insert

        public JsonResult CreditServices_Insert(AddCreditServiceRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CreditService_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _creditServiceService.AddCreditServices(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit
        public JsonResult CreditServices_Edit(EditCreditServiceRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CreditService_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _creditServiceService.EditCreditServices(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Delete

        public JsonResult CreditServices_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CreditService_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _creditServiceService.DeleteCreditServices(requests);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Moveing

        public JsonResult CreditService_MoveUp(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _creditServiceService.MoveUp(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreditService_MoveDown(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _creditServiceService.MoveDown(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Private Members

        private CreditServiceView GetCreditServiceView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetCreditServiceResponse response = this._creditServiceService.GetCreditService(request);

            return response.CreditServiceView;
        }

        #endregion

    }
}
