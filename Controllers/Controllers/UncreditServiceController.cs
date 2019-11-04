using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.StoreCatalog;
using System.Web.Mvc;
using Services.ViewModels.Employees;
using Services.ViewModels.Store;
using Services.Messaging;
using Services.Messaging.StoreCatalogService;
using Controllers.ViewModels;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Controllers.Controllers
{
    [Authorize]
    public class UncreditServiceController: BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly IUncreditServiceService _uncreditServiceService;
        #endregion

        #region Ctor
        public UncreditServiceController(IEmployeeService employeeService, IUncreditServiceService uncreditServiceService)
            : base(employeeService)
        {
            this._uncreditServiceService = uncreditServiceService;
            this._employeeService = employeeService;
        }
        #endregion

        #region Old Methods

        #region Ajax Read
        public ActionResult UncreditService_Read([DataSourceRequest] DataSourceRequest request)
        {
            UncreditServiceHomePageView uncreditServiceHomePageView = new UncreditServiceHomePageView();
            uncreditServiceHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("UncreditService_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(uncreditServiceHomePageView.UncreditServiceViews.ToDataSourceResult(request));
            }
            #endregion

            uncreditServiceHomePageView.UncreditServiceViews = this._uncreditServiceService.GetUncreditServices().UncreditServiceViews;

            return Json(uncreditServiceHomePageView.UncreditServiceViews.ToDataSourceResult(request));
        }
        #endregion

        #region Index
        public ActionResult Index()
        {
            UncreditServiceHomePageView uncreditServiceHomePageView = new UncreditServiceHomePageView();
            uncreditServiceHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("UncreditService_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(uncreditServiceHomePageView);
            }
            #endregion
            
            uncreditServiceHomePageView.UncreditServiceViews = this._uncreditServiceService.GetUncreditServices().UncreditServiceViews;

            return View(uncreditServiceHomePageView);
        }
        #endregion

        

        #region Create
        public ActionResult Create()
        {
            UncreditServiceDetailView uncreditServiceDetailView = new UncreditServiceDetailView();
            uncreditServiceDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("UncreditService_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(uncreditServiceDetailView);
            }
            #endregion

            return View(uncreditServiceDetailView);
        }

        [HttpPost]
        public ActionResult Create(UncreditServiceDetailView uncreditServiceDetailView)
        {
            uncreditServiceDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("UncreditService_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(uncreditServiceDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddUncreditServiceRequestOld request = new AddUncreditServiceRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.Discontinued = uncreditServiceDetailView.UncreditServiceView.Discontinued;
                    request.Imposition = uncreditServiceDetailView.UncreditServiceView.Imposition;
                    request.MaxDiscount = uncreditServiceDetailView.UncreditServiceView.MaxDiscount;
                    request.SortOrder = uncreditServiceDetailView.UncreditServiceView.SortOrder;
                    request.UnCreditServiceCode = uncreditServiceDetailView.UncreditServiceView.UnCreditServiceCode;
                    request.Note = uncreditServiceDetailView.UncreditServiceView.Note;
                    request.UncreditServiceName = uncreditServiceDetailView.UncreditServiceView.UncreditServiceName;
                    request.UnitPrice = uncreditServiceDetailView.UncreditServiceView.UnitPrice;

                    GeneralResponse response = this._uncreditServiceService.AddUncreditService(request);

                    if (!response.hasError)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(uncreditServiceDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(uncreditServiceDetailView);
                }

            return View(uncreditServiceDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            UncreditServiceDetailView uncreditServiceDetailView = new UncreditServiceDetailView();
            uncreditServiceDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("UncreditService_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(uncreditServiceDetailView);
            }
            #endregion

            uncreditServiceDetailView.UncreditServiceView = this.GetUncreditServiceView(id);
            
            return View(uncreditServiceDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, UncreditServiceDetailView uncreditServiceDetailView)
        {
            uncreditServiceDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("UncreditService_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(uncreditServiceDetailView);
            }
            #endregion
            
            if (ModelState.IsValid)
                try
                {
                    EditUncreditServiceRequestOld request = new EditUncreditServiceRequestOld();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.Discontinued = uncreditServiceDetailView.UncreditServiceView.Discontinued;
                    request.Imposition = uncreditServiceDetailView.UncreditServiceView.Imposition;
                    request.MaxDiscount = uncreditServiceDetailView.UncreditServiceView.MaxDiscount;
                    request.SortOrder = uncreditServiceDetailView.UncreditServiceView.SortOrder;
                    request.UnCreditServiceCode = uncreditServiceDetailView.UncreditServiceView.UnCreditServiceCode;
                    request.Note = uncreditServiceDetailView.UncreditServiceView.Note;
                    request.UncreditServiceName = uncreditServiceDetailView.UncreditServiceView.UncreditServiceName;
                    request.UnitPrice = uncreditServiceDetailView.UncreditServiceView.UnitPrice;
                    request.RowVersion = uncreditServiceDetailView.UncreditServiceView.RowVersion;

                    GeneralResponse response = this._uncreditServiceService.EditUncreditService(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(uncreditServiceDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(uncreditServiceDetailView);
                }

            return View(uncreditServiceDetailView);
        }
        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            UncreditServiceDetailView uncreditServiceDetailView = new UncreditServiceDetailView();
            uncreditServiceDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("UncreditService_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(uncreditServiceDetailView);
            }
            #endregion

            UncreditServiceView uncreditServiceView = this.GetUncreditServiceView(id);

            uncreditServiceDetailView.UncreditServiceView = uncreditServiceView;
            
            return View(uncreditServiceDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            UncreditServiceDetailView uncreditServiceDetailView = new UncreditServiceDetailView();
            uncreditServiceDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("UncreditService_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(uncreditServiceDetailView);
            }
            #endregion

            uncreditServiceDetailView.UncreditServiceView = this.GetUncreditServiceView(id);
            
            return View(uncreditServiceDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            UncreditServiceDetailView uncreditServiceDetailView = new UncreditServiceDetailView();
            uncreditServiceDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("UncreditService_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(uncreditServiceDetailView);
            }
            #endregion

            uncreditServiceDetailView.UncreditServiceView = this.GetUncreditServiceView(id);
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._uncreditServiceService.DeleteUncreditService(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(uncreditServiceDetailView);
            }
        }
        #endregion

        #endregion

        #region new Methods

        #region read

        public JsonResult Uncreditservices_Read(int? pageSize, int? pageNumber, string sort)
        {
            GetGeneralResponse<IEnumerable<UncreditServiceView>> response = new GetGeneralResponse<IEnumerable<UncreditServiceView>>();

            #region Access Check

            EmployeeView employee = GetEmployee();
            bool hasPermission = employee.IsGuaranteed("UncreditService_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _uncreditServiceService.GetUncreditServices(PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Uncreditservices_Reads(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<UncreditServiceView>> response = new GetGeneralResponse<IEnumerable<UncreditServiceView>>();

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _uncreditServiceService.GetUncreditServices(PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult Uncreditservices_Insert(AddUncreditServiceRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("UncreditService_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _uncreditServiceService.AddUncreditServices(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit
        //1392/11/09 تست شده
        public JsonResult Uncreditservices_Update(EditUncreditServiceRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            EmployeeView employee = GetEmployee();
            bool hasPermission = employee.IsGuaranteed("UncreditService_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _uncreditServiceService.EditUncreditServices(request, employee.ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult UncreditServices_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("UncreditService_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _uncreditServiceService.DeleteUncreditServices(requests);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Moving

        public ActionResult UncreditService_MoveUp(string ID)
        {
            GeneralResponse response=new GeneralResponse();
            MoveRequest request = new MoveRequest();
            request.ID = Guid.Parse(ID);

            MoveResponse move = _uncreditServiceService.MoveUp(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UncreditService_MoveDown(string ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveRequest request = new MoveRequest();
            request.ID = Guid.Parse(ID);

            MoveResponse move = _uncreditServiceService.MoveDown(request);

            return Json(response, JsonRequestBehavior.AllowGet); 
        }

        #endregion

        #endregion

        #region Private Members

        private UncreditServiceView GetUncreditServiceView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetUncreditServiceResponse response = this._uncreditServiceService.GetUncreditService(request);

            return response.UncreditServiceView;
        }

        #endregion

    }
}
