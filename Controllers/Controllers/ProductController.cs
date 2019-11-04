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
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class ProductController: BaseController
    {
        #region Declares
        
        private readonly IEmployeeService _employeeService;

        private readonly IProductService _productService;

        #endregion

        #region Ctor

        public ProductController(IEmployeeService employeeService, IProductService productService)
            : base(employeeService)
        {
            this._productService = productService;
            this._employeeService = employeeService;
        }

        #endregion

        #region Old Methods

        #region Ajax Read

        public ActionResult Product_Read([DataSourceRequest] DataSourceRequest request)
        {
            ProductHomePageView productHomePageView = new ProductHomePageView();
            productHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Product_Read");
            if (!hasPermission)
            {
                GetProductsResponse temp = new GetProductsResponse();
                var _result = new DataSourceResult()
                {
                    Data = temp.ProductViews,
                    Total = temp.Count
                };
                ModelState.AddModelError("", "AccessDenied");
                return Json(_result);
            }
            #endregion

            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            getRequest.PageNumber = request.Page;
            GetProductsResponse productResponse = this._productService.GetProducts(getRequest);

            productHomePageView.ProductViews = productResponse.ProductViews;
            productHomePageView.Count = productResponse.Count;

            var result = new DataSourceResult()
            {
                Data = productResponse.ProductViews,
                Total = productResponse.Count
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Index
        
        public ActionResult Index()
        {
            ProductHomePageView productHomePageView = new ProductHomePageView();
            productHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Product_Read");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(productHomePageView);
            }
            #endregion

            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = 10;
            getRequest.PageNumber = 1;

            GetProductsResponse productResponse = this._productService.GetProducts(getRequest);
            //GetProductsResponse productResponse = this._productService.GetProducts();
            
            productHomePageView.ProductViews = productResponse.ProductViews;
            productHomePageView.Count = productResponse.Count;

            DataSourceRequest request = new DataSourceRequest
            {
                PageSize = 10,
                Page = 1
            };

            Product_Read(request);

            return View(productHomePageView);
        }

        #endregion

        #region Create
        public ActionResult Create()
        {
            ProductDetailView productDetailView = new ProductDetailView();
            productDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Product_Insert");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(productDetailView);
            }
            #endregion
            
            return View(productDetailView);
        }

        [HttpPost]
        public ActionResult Create(ProductDetailView productDetailView)
        {
            productDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Product_Insert");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(productDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddProductRequestOld request = new AddProductRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.Discontinued = productDetailView.ProductView.Discontinued;
                    request.ProductName = productDetailView.ProductView.ProductName;
                    request.ProductCategoryID = productDetailView.ProductView.ProductCategoryID;
                    request.ProductCode = productDetailView.ProductView.ProductCode;
                    request.Note = productDetailView.ProductView.Note;
                    request.UnitsInStock = productDetailView.ProductView.UnitsInStock;

                    GeneralResponse response = this._productService.AddProduct(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(productDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(productDetailView);
                }

            return View(productDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            ProductDetailView productDetailView = new ProductDetailView();
            productDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Product_Update");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(productDetailView);
            }
            #endregion

            productDetailView.ProductView = this.GetProductView(id);
            
            return View(productDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, ProductDetailView productDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Product_Update");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(productDetailView);
            }
            #endregion

            productDetailView.EmployeeView = GetEmployee();

            if (ModelState.IsValid)
                try
                {
                    EditProductRequestOld request = new EditProductRequestOld();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.Discontinued = productDetailView.ProductView.Discontinued;
                    request.ProductName = productDetailView.ProductView.ProductName;
                    request.Discontinued = productDetailView.ProductView.Discontinued;
                    request.ProductCategoryID = productDetailView.ProductView.ProductCategoryID;
                    request.ProductCode = productDetailView.ProductView.ProductCode;
                    request.Note = productDetailView.ProductView.Note;
                    request.UnitsInStock = productDetailView.ProductView.UnitsInStock;
                    request.RowVersion = productDetailView.ProductView.RowVersion;

                    GeneralResponse response = this._productService.EditProduct(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(productDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(productDetailView);
                }

            return View(productDetailView);
        }
        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            ProductDetailView productDetailView = new ProductDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Product_Read");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(productDetailView);
            }
            #endregion

            ProductView productView = this.GetProductView(id);

            productDetailView.ProductView = productView;
            productDetailView.EmployeeView = GetEmployee();

            return View(productDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            ProductDetailView productDetailView = new ProductDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Product_Delete");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(productDetailView);
            }
            #endregion

            productDetailView.ProductView = this.GetProductView(id);
            productDetailView.EmployeeView = GetEmployee();

            return View(productDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            ProductDetailView productDetailView = new ProductDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Product_Delete");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(productDetailView);
            }
            #endregion

            productDetailView.ProductView = this.GetProductView(id);
            productDetailView.EmployeeView = GetEmployee();

            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._productService.DeleteProduct(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(productDetailView);
            }
        }
        #endregion

        #endregion

        #region New Methods

        #region Read

        public JsonResult Products_Read(Guid ProductID)
        {
            GetGeneralResponse<ProductView> response = new GetGeneralResponse<ProductView>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Product_Read");
            if (!hasPermission)
            {

                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion  

            response = _productService.GetProduct(ProductID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Productss_Read(bool Discontinued , int? pageSize, int? pageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<ProductView>> response = new GetGeneralResponse<IEnumerable<ProductView>>();

            //#region Access Check
            //bool hasPermission = GetEmployee().IsGuaranteed("Product_Read");
            //if (!hasPermission)
            //{

            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
            //#endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _productService.GetProducts(Discontinued, PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Productss_Reads(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<ProductView>> response = new GetGeneralResponse<IEnumerable<ProductView>>();



            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _productService.GetProducts(PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert


        public JsonResult Products_Insert(IEnumerable<AddProductRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Product_Insert");
            if (!hasPermission)
            {

                response.ErrorMessages.Add("AccessDenied");
                return Json(response,JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _productService.AddProduct(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region Update

        public JsonResult Products_Update(IEnumerable<EditProductRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Product_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _productService.EditProducts(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult Products_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Product_Update");
            if (!hasPermission)
            {

                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _productService.DeleteProducts(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Moveing

        public JsonResult Product_MoveUp(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _productService.MoveUp(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Product_MoveDown(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _productService.MoveDown(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Private Members

        private ProductView GetProductView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetProductResponse response = this._productService.GetProduct(request);

            return response.ProductView;
        }

        #endregion

    }
}
