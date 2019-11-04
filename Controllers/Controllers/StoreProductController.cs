#region Usings
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
using Controllers.ViewModels;
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class StoreProductController: BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly IStoreProductService _storeProductService;
        private readonly IProductService _productService;
        private readonly IStoreService _storeService;
        #endregion

        #region Ctor
        public StoreProductController(IEmployeeService employeeService, IStoreProductService storeProductService
            , IProductService productService, IStoreService storeService)
            : base(employeeService)
        {
            _storeProductService = storeProductService;
            _productService = productService;
            _employeeService = employeeService;
            _storeService = storeService;
        }
        #endregion

        //#region Ajax Read
        //public ActionResult StoreProduct_Read(string id, [DataSourceRequest] DataSourceRequest request)
        //{
        //    StoreProductHomePageView storeProductHomePageView = new StoreProductHomePageView();
        //    storeProductHomePageView.EmployeeView = GetEmployee();
        //    storeProductHomePageView.StoreProductViews = _storeProductService.GetStoreProducts(Guid.Parse(id)).StoreProductViews;
        //    AjaxGetRequest getRequest = new AjaxGetRequest();

        //    getRequest.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
        //    getRequest.PageNumber = request.Page;
        //    GetStoreProductsResponse storeProductResponse = this._storeProductService.GetStoreProducts(getRequest);

        //    storeProductHomePageView.StoreProductViews = storeProductResponse.StoreProductViews;
        //    storeProductHomePageView.Count = storeProductResponse.Count;

        //    var result = new DataSourceResult()
        //    {
        //        Data = storeProductResponse.StoreProductViews,
        //        Total = storeProductResponse.Count
        //    };
        //    return Json(result);
        //}
        //#endregion

        #region Index
        //public ActionResult Index(string id)
        //{
        //    StoreProductHomePageView storeProductHomePageView = new StoreProductHomePageView();
        //    storeProductHomePageView.EmployeeView = GetEmployee();

        //    #region Access Check
        //    bool hasPermission = GetEmployee().IsGuaranteed("StoreProduct_Read");
        //    if (!hasPermission)
        //    {
        //        ModelState.AddModelError("", "AccessDenied");
        //        return View(storeProductHomePageView);
        //    }
        //    #endregion

        //    storeProductHomePageView.StoreProductViews = this._storeProductService.GetStoreProducts(Guid.Parse(id)).StoreProductViews;

        //    GetRequest getStoreRequest = new GetRequest() { ID = Guid.Parse(id) };
        //    storeProductHomePageView.StoreView = this._storeService.GetStore(getStoreRequest).StoreView;

        //    //GetRequest getProductRequest = new GetRequest();
        //    //getProductRequest.ID = Guid.Parse(id);
        //    //storeProductHomePageView.ProductView = _productService.GetProduct(getProductRequest).ProductView;

        //    return View(storeProductHomePageView);
        //}
        #endregion

        #region Ajax
        public JsonResult AjaxProductUnitsInStock(string prID)
        {
            ProductView productView = new ProductView();

            #region Access Check
            // Commented By Hojjat

            //bool hasPermission = GetEmployee().IsGuaranteed("StoreProduct_Read");
            //if (!hasPermission)
            //{
            //    var _result = new { success = "True", UnitsInStock = productView.UnitsInStock };
            //    ModelState.AddModelError("", "AccessDenied");
            //    return Json(_result, JsonRequestBehavior.AllowGet);
            //}
            #endregion

            productView = _productService.GetProduct(new GetRequest() { ID = Guid.Parse(prID) }).ProductView;

            var result = new { success = "True", UnitsInStock = productView.UnitsInStock };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Create
        public ActionResult Create(string id)
        {
            StoreProductDetailView storeProductDetailView = new StoreProductDetailView();
            storeProductDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("StoreProduct_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(storeProductDetailView);
            }
            #endregion

            GetRequest getStoreRequest = new GetRequest() { ID = Guid.Parse(id) };
            storeProductDetailView.StoreView = this._storeService.GetStore(getStoreRequest).StoreView;

            #region DropDownList For Products
            storeProductDetailView.ProductViews = _productService.GetProducts().ProductViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (storeProductDetailView.ProductViews != null)
                foreach (ProductView productView in storeProductDetailView.ProductViews)
                {
                    list.Add(new DropDownItem { Value = productView.ID.ToString(), Text = productView.ProductName });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["Products"] = selectList;
            #endregion

            return View(storeProductDetailView);
        }

        [HttpPost]
        public ActionResult Create(string id, StoreProductDetailView storeProductDetailView)
        {
            storeProductDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("StoreProduct_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(storeProductDetailView);
            }
            #endregion

            GetRequest getStoreRequest = new GetRequest() { ID = Guid.Parse(id) };
            storeProductDetailView.StoreView = this._storeService.GetStore(getStoreRequest).StoreView;

            #region DropDownList For Products
            storeProductDetailView.ProductViews = _productService.GetProducts().ProductViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (storeProductDetailView.ProductViews != null)
                foreach (ProductView productView in storeProductDetailView.ProductViews)
                {
                    list.Add(new DropDownItem { Value = productView.ID.ToString(), Text = productView.ProductName });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["Products"] = selectList;
            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddStoreProductRequest request = new AddStoreProductRequest();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.ProductID = storeProductDetailView.StoreProductView.ProductID;
                    request.StoreID = Guid.Parse(id);
                    request.UnitsInStock = storeProductDetailView.StoreProductView.UnitsInStock;

                    GeneralResponse response = this._storeProductService.AddStoreProduct(request);

                    if (response.success)
                        return RedirectToAction("Index/" + request.StoreID);
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(storeProductDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(storeProductDetailView);
                }

            return View(storeProductDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id, string productID)
        {
            StoreProductDetailView storeProductDetailView = new StoreProductDetailView();
            storeProductDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("StoreProduct_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(storeProductDetailView);
            }
            #endregion

            storeProductDetailView.StoreProductView = this.GetStoreProductView(id, productID);
            
            return View(storeProductDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, StoreProductDetailView storeProductDetailView)
        {
            storeProductDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("StoreProduct_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(storeProductDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditStoreProductRequest request = new EditStoreProductRequest();

                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.ProductID = storeProductDetailView.StoreProductView.ProductID;
                    request.StoreID = storeProductDetailView.StoreProductView.StoreID;
                    request.UnitsInStock = storeProductDetailView.StoreProductView.UnitsInStock;
                    request.RowVersion = storeProductDetailView.StoreProductView.RowVersion;

                    GeneralResponse response = this._storeProductService.EditStoreProduct(request);

                    if (response.success)
                        return RedirectToAction("Index/" + request.StoreID);
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(storeProductDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(storeProductDetailView);
                }

            return View(storeProductDetailView);
        }
        #endregion

        #region Details
        public ActionResult Details(string id, string productID)
        {
            StoreProductView storeProductView = this.GetStoreProductView(id, productID);
            StoreProductDetailView storeProductDetailView = new StoreProductDetailView();
            storeProductDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("StoreProduct_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(storeProductView);
            }
            #endregion
          
            storeProductDetailView.StoreProductView = storeProductView;
            
            return View(storeProductDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id, string productID)
        {
            StoreProductDetailView storeProductDetailView = new StoreProductDetailView();
            storeProductDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("StoreProduct_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(storeProductDetailView);
            }
            #endregion
            
            storeProductDetailView.StoreProductView = this.GetStoreProductView(id, productID);
            
            return View(storeProductDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, string productID, FormCollection collection)
        {
            StoreProductDetailView storeProductDetailView = new StoreProductDetailView();
            storeProductDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("StoreProduct_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(storeProductDetailView);
            }
            #endregion
            
            storeProductDetailView.StoreProductView = this.GetStoreProductView(id, productID);
            
            GeneralResponse response = this._storeProductService.DeleteStoreProduct(Guid.Parse(id), Guid.Parse(productID));

            if (response.success)
                return RedirectToAction("Index/" + storeProductDetailView.StoreProductView.StoreID);
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(storeProductDetailView);
            }
        }
        #endregion

        #region new Methods

        public JsonResult StoreProduct_Read_ByStore(Guid StoreID)
        {
            GetGeneralResponse<IEnumerable<StoreProductView>> response = new GetGeneralResponse<IEnumerable<StoreProductView>>();

            response = _storeProductService.GetStoreProducts(StoreID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StoreProduct_Read_Product(Guid ProductID)
        {
            GetGeneralResponse<IEnumerable<StoreProductView>> response = new GetGeneralResponse<IEnumerable<StoreProductView>>();

            response = _storeProductService.GetProductStore(ProductID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Members

        private StoreProductView GetStoreProductView(string storeID, string productID)
        {
            GetStoreProductResponse response = this._storeProductService.GetStoreProduct(Guid.Parse(storeID), Guid.Parse(productID));

            return response.StoreProductView;
        }

        #endregion

    }
}
