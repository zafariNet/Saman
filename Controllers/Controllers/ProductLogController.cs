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
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Controllers.Controllers
{
    [Authorize]
    public class ProductLogController: BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly IProductLogService _productLogService;

        private readonly IProductService _productService;
        #endregion

        #region Ctor
        public ProductLogController(IEmployeeService employeeService, IProductLogService productLogService
            , IProductService productService)
            : base(employeeService)
        {
            this._productLogService = productLogService;
            this._productService = productService;
            this._employeeService = employeeService;
        }
        #endregion

        #region Old Methods

        #region Ajax Read
        public ActionResult ProductLog_Read([DataSourceRequest] DataSourceRequest request)
        {
            ProductLogHomePageView productLogHomePageView = new ProductLogHomePageView();
            productLogHomePageView.EmployeeView = GetEmployee();
            productLogHomePageView.ProductLogViews = this._productLogService.GetProductLogs().ProductLogViews;

            return Json(productLogHomePageView.ProductLogViews.ToDataSourceResult(request));
        }
        #endregion

        #region Index
        public ActionResult Index(string id)
        {
            ProductLogHomePageView productLogHomePageView = new ProductLogHomePageView();
            productLogHomePageView.EmployeeView = GetEmployee();
            productLogHomePageView.ProductLogViews = this._productLogService.GetProductLogs(Guid.Parse(id)).ProductLogViews;

            GetRequest request = new GetRequest() { ID = Guid.Parse(id) };
            productLogHomePageView.ProductView = _productService.GetProduct(request).ProductView;

            return View(productLogHomePageView);
        }
        #endregion

        #region Create
        public ActionResult Create(string id)
        {
            ProductLogDetailView productLogDetailView = new ProductLogDetailView();
            productLogDetailView.EmployeeView = GetEmployee();

            GetRequest request = new GetRequest() { ID = Guid.Parse(id) };
            try
            {
                ProductView productView = new ProductView();
                productView = _productService.GetProduct(request).ProductView;

                productLogDetailView.ProductLogView = new ProductLogView();
                productLogDetailView.ProductLogView.ProductID = productView.ID;
                productLogDetailView.ProductLogView.ProductName = productView.ProductName;
            }
            catch (Exception ex)
            {
                throw;
            }
            
            return View(productLogDetailView);
        }

        [HttpPost]
        public ActionResult Create(ProductLogDetailView productLogDetailView)
        {
            productLogDetailView.EmployeeView = GetEmployee();

            GetRequest getCenterRequest = new GetRequest() { ID = productLogDetailView.ProductLogView.ProductID };
            productLogDetailView.ProductLogView.ProductName = _productService.GetProduct(getCenterRequest).ProductView.ProductName;

            if (ModelState.IsValid)
                try
                {
                    AddProductLogRequestOld request = new AddProductLogRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.Closed = productLogDetailView.ProductLogView.Closed;
                    request.InputSerialNumber = productLogDetailView.ProductLogView.InputSerialNumber;
                    request.LogDate = productLogDetailView.ProductLogView.LogDate;
                    request.Note = productLogDetailView.ProductLogView.Note;
                    request.ProductID = productLogDetailView.ProductLogView.ProductID;
                    request.ProductSerialFrom = productLogDetailView.ProductLogView.ProductSerialFrom;
                    request.ProductSerialTo = productLogDetailView.ProductLogView.ProductSerialTo;
                    request.PurchaseBillNumber = productLogDetailView.ProductLogView.PurchaseBillNumber;
                    request.PurchaseDate = productLogDetailView.ProductLogView.PurchaseDate;
                    request.PurchaseUnitPrice = productLogDetailView.ProductLogView.PurchaseUnitPrice;
                    request.SellerName = productLogDetailView.ProductLogView.SellerName;

                    if (productLogDetailView.ProductLogView.IOTypeForCreate == "O")
                        request.UnitsIO = -Math.Abs(productLogDetailView.ProductLogView.UnitsIO);
                    else
                        request.UnitsIO = Math.Abs(productLogDetailView.ProductLogView.UnitsIO);

                    GeneralResponse response = this._productLogService.AddProductLog(request);

                    if (response.success)
                        return RedirectToAction("Index/" + request.ProductID);
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(productLogDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(productLogDetailView);
                }

            return View(productLogDetailView);
        }

        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            ProductLogDetailView productLogDetailView = new ProductLogDetailView();
            productLogDetailView.ProductLogView = this.GetProductLogView(id);
            productLogDetailView.EmployeeView = GetEmployee();

            productLogDetailView.ProductLogView.IOTypeForCreate = productLogDetailView.ProductLogView.UnitsIO >= 0 ? "I" : "O";
            productLogDetailView.ProductLogView.UnitsIO = Math.Abs(productLogDetailView.ProductLogView.UnitsIO);

            GetRequest getCenterRequest = new GetRequest() { ID = productLogDetailView.ProductLogView.ProductID };
            productLogDetailView.ProductLogView.ProductName = _productService.GetProduct(getCenterRequest).ProductView.ProductName;

            return View(productLogDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, ProductLogDetailView productLogDetailView)
        {
            productLogDetailView.EmployeeView = GetEmployee();

            if (ModelState.IsValid)
                try
                {
                    EditProductLogRequest request = new EditProductLogRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.Closed = productLogDetailView.ProductLogView.Closed;
                    request.InputSerialNumber = productLogDetailView.ProductLogView.InputSerialNumber;
                    request.LogDate = productLogDetailView.ProductLogView.LogDate;
                    request.Note = productLogDetailView.ProductLogView.Note;
                    request.ProductID = productLogDetailView.ProductLogView.ProductID;
                    request.ProductSerialFrom = productLogDetailView.ProductLogView.ProductSerialFrom;
                    request.ProductSerialTo = productLogDetailView.ProductLogView.ProductSerialTo;
                    request.PurchaseBillNumber = productLogDetailView.ProductLogView.PurchaseBillNumber;
                    request.PurchaseDate = productLogDetailView.ProductLogView.PurchaseDate;
                    request.PurchaseUnitPrice = productLogDetailView.ProductLogView.PurchaseUnitPrice;
                    request.SellerName = productLogDetailView.ProductLogView.SellerName;
                    request.TotalLine = productLogDetailView.ProductLogView.TotalLine;
                    if (productLogDetailView.ProductLogView.IOTypeForCreate == "O")

                        request.UnitsIO = -Math.Abs(productLogDetailView.ProductLogView.UnitsIO);
                    else
                        request.UnitsIO = Math.Abs(productLogDetailView.ProductLogView.UnitsIO);

                    request.RowVersion = productLogDetailView.ProductLogView.RowVersion;

                    GeneralResponse response = this._productLogService.EditProductLog(request);

                    if (response.success)
                        return RedirectToAction("Index/" + request.ProductID);
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(productLogDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(productLogDetailView);
                }

            return View(productLogDetailView);
        }
        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            ProductLogView productLogView = this.GetProductLogView(id);

            ProductLogDetailView productLogDetailView = new ProductLogDetailView();
            productLogDetailView.ProductLogView = productLogView;
            productLogDetailView.EmployeeView = GetEmployee();

            GetRequest getCenterRequest = new GetRequest() { ID = productLogDetailView.ProductLogView.ProductID };
            productLogDetailView.ProductLogView.ProductName = _productService.GetProduct(getCenterRequest).ProductView.ProductName;

            return View(productLogDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            ProductLogDetailView productLogDetailView = new ProductLogDetailView();
            productLogDetailView.ProductLogView = this.GetProductLogView(id);
            productLogDetailView.EmployeeView = GetEmployee();

            return View(productLogDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            ProductLogDetailView productLogDetailView = new ProductLogDetailView();
            productLogDetailView.ProductLogView = this.GetProductLogView(id);
            productLogDetailView.EmployeeView = GetEmployee();

            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._productLogService.DeleteProductLog(request);

            if (response.success)
                return RedirectToAction("Index/" + productLogDetailView.ProductLogView.ProductID);
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(productLogDetailView);
            }
        }
        #endregion

        #endregion

        #region New Methods

        #region Read All

        public JsonResult ProductLogs_Read_All(int? pageSize, int? pageNumber)
        {
            GeneralResponse response = new GeneralResponse();

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _productLogService.GetProductLogs(PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read By Product

        public JsonResult productLogs_read_ByProduct(Guid ProductID, int? pageSize, int? pageNumber,string sort)
        {
            GeneralResponse response = new GeneralResponse();

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _productLogService.GetProductLogsByProduct(ProductID, PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Read By store

        public JsonResult ProductLogs_Transfer_Read(Guid ProductID, int? pageSize, int? pageNumber,string sort)
        {
            GeneralResponse response = new GeneralResponse();

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;


            response = _productLogService.GetProductLogsByProductInStore(ProductID, PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Insert

        public JsonResult ProductLog_Insert(AddProductLogRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("ProductLog_Transfer");
            if (!hasPermission)
            {

                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _productLogService.AddProductLog(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ProductLogs_Transfer_Insert(AddProductLogStoreRequest request)
        {
            GeneralResponse response = new GeneralResponse();


            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("ProductLog_Transfer");
            if (!hasPermission)
            {

                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _productLogService.AddProductLogToStore(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #endregion

        #region Private Members

        private ProductLogView GetProductLogView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetProductLogResponse response = this._productLogService.GetProductLog(request);

            return response.ProductLogView;
        }

        #endregion

    }
}

