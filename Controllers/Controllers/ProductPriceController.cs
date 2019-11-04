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
    public class ProductPriceController: BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly IProductPriceService _productPriceService;

        private readonly IProductService _productService;
        #endregion

        #region Ctor
        public ProductPriceController(IEmployeeService employeeService, IProductPriceService productPriceService
            , IProductService productService)
            : base(employeeService)
        {
            this._productPriceService = productPriceService;
            this._productService = productService;
            this._employeeService = employeeService;
        }
        #endregion

        #region Old Methods

        #region Ajax Read
        public ActionResult ProductPrice_Read([DataSourceRequest] DataSourceRequest request)
        {
            ProductPriceHomePageView productPriceHomePageView = new ProductPriceHomePageView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductPrice_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                Json(productPriceHomePageView.ProductPriceViews.ToDataSourceResult(request));
            }
            #endregion
            
            productPriceHomePageView.EmployeeView = GetEmployee();
            productPriceHomePageView.ProductPriceViews = this._productPriceService.GetProductPrices().ProductPriceViews;

            return Json(productPriceHomePageView.ProductPriceViews.ToDataSourceResult(request));
        }
        #endregion

        #region Index
        public ActionResult Index(string id)
        {
            ProductPriceHomePageView productPriceHomePageView = new ProductPriceHomePageView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductPrice_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productPriceHomePageView);
            }
            #endregion
            
            productPriceHomePageView.EmployeeView = GetEmployee();
            productPriceHomePageView.ProductPriceViews = this._productPriceService.GetProductPrices(Guid.Parse(id)).ProductPriceViews;

            GetRequest request = new GetRequest() { ID = Guid.Parse(id) };
            productPriceHomePageView.ProductView = _productService.GetProduct(request).ProductView;

            return View(productPriceHomePageView);
        }
        #endregion

        #region Create
        public ActionResult Create(string id)
        {
            ProductPriceDetailView productPriceDetailView = new ProductPriceDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductPrice_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productPriceDetailView);
            }
            #endregion

            productPriceDetailView.EmployeeView = GetEmployee();

            GetRequest request = new GetRequest() { ID = Guid.Parse(id) };
            try
            {
                ProductView productView = new ProductView();
                productView = _productService.GetProduct(request).ProductView;

                productPriceDetailView.ProductPriceView = new ProductPriceView();
                productPriceDetailView.ProductPriceView.ProductID = productView.ID;
                productPriceDetailView.ProductPriceView.ProductName = productView.ProductName;
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(productPriceDetailView);
        }

        [HttpPost]
        public ActionResult Create(ProductPriceDetailView productPriceDetailView)
        {
            
            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductPrice_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productPriceDetailView);
            }
            #endregion

            productPriceDetailView.EmployeeView = GetEmployee();

            GetRequest getCenterRequest = new GetRequest() { ID = productPriceDetailView.ProductPriceView.ProductID };
            productPriceDetailView.ProductPriceView.ProductName = _productService.GetProduct(getCenterRequest).ProductView.ProductName;

            if (ModelState.IsValid)
                try
                {
                    AddProductPriceRequestOld request = new AddProductPriceRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.Discontinued = productPriceDetailView.ProductPriceView.Discontinued;
                    request.Imposition = productPriceDetailView.ProductPriceView.Imposition;
                    request.MaxDiscount = productPriceDetailView.ProductPriceView.MaxDiscount;
                    request.Note = productPriceDetailView.ProductPriceView.Note;
                    request.ProductID = productPriceDetailView.ProductPriceView.ProductID;
                    request.ProductPriceTitle = productPriceDetailView.ProductPriceView.ProductPriceTitle;
                    request.SortOrder = productPriceDetailView.ProductPriceView.SortOrder;
                    request.UnitPrice = productPriceDetailView.ProductPriceView.UnitPrice;

                    GeneralResponse response = this._productPriceService.AddProductPrice(request);

                    if (response.success)
                        return RedirectToAction("Index/" + request.ProductID);
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(productPriceDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(productPriceDetailView);
                }

            return View(productPriceDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            ProductPriceDetailView productPriceDetailView = new ProductPriceDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductPrice_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productPriceDetailView);
            }
            #endregion

            productPriceDetailView.ProductPriceView = this.GetProductPriceView(id);
            productPriceDetailView.EmployeeView = GetEmployee();

            return View(productPriceDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, ProductPriceDetailView productPriceDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductPrice_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productPriceDetailView);
            }
            #endregion

            productPriceDetailView.EmployeeView = GetEmployee();

            if (ModelState.IsValid)
                try
                {
                    EditProductPriceRequestOld request = new EditProductPriceRequestOld();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.Discontinued = productPriceDetailView.ProductPriceView.Discontinued;
                    request.Imposition = productPriceDetailView.ProductPriceView.Imposition;
                    request.MaxDiscount = productPriceDetailView.ProductPriceView.MaxDiscount;
                    request.Note = productPriceDetailView.ProductPriceView.Note;
                    request.ProductID = productPriceDetailView.ProductPriceView.ProductID;
                    request.ProductPriceTitle = productPriceDetailView.ProductPriceView.ProductPriceTitle;
                    request.SortOrder = productPriceDetailView.ProductPriceView.SortOrder;
                    request.UnitPrice = productPriceDetailView.ProductPriceView.UnitPrice;
                    request.RowVersion = productPriceDetailView.ProductPriceView.RowVersion;

                    GeneralResponse response = this._productPriceService.EditProductPrice(request);

                    if (response.success)
                        return RedirectToAction("Index/" + request.ProductID);
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(productPriceDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(productPriceDetailView);
                }

            return View(productPriceDetailView);
        }
        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            ProductPriceDetailView productPriceDetailView = new ProductPriceDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductPrice_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productPriceDetailView);
            }
            #endregion

            ProductPriceView productPriceView = this.GetProductPriceView(id);

            productPriceDetailView.ProductPriceView = productPriceView;
            productPriceDetailView.EmployeeView = GetEmployee();

            return View(productPriceDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            ProductPriceDetailView productPriceDetailView = new ProductPriceDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductPrice_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productPriceDetailView);
            }
            #endregion

            productPriceDetailView.ProductPriceView = this.GetProductPriceView(id);
            productPriceDetailView.EmployeeView = GetEmployee();

            return View(productPriceDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            ProductPriceDetailView productPriceDetailView = new ProductPriceDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductPrice_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productPriceDetailView);
            }
            #endregion

            productPriceDetailView.ProductPriceView = this.GetProductPriceView(id);
            productPriceDetailView.EmployeeView = GetEmployee();

            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._productPriceService.DeleteProductPrice(request);

            if (response.success)
                return RedirectToAction("Index/" + productPriceDetailView.ProductPriceView.ProductID);
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(productPriceDetailView);
            }
        }
        #endregion

        #endregion

        #region new Methods

        #region Read

        #region Read All 
        public JsonResult ProductPrices_Read(Guid? ProductID,int? pageSize, int? pageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<ProductPriceView>> response = new GetGeneralResponse<IEnumerable<ProductPriceView>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductPrice_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion


            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            Guid productID=ProductID==null?Guid.Empty:(Guid)ProductID;

            response = _productPriceService.GetProductPrices(productID,PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ProductPrices_Reads(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<ProductPriceView>> response = new GetGeneralResponse<IEnumerable<ProductPriceView>>();

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _productPriceService.GetProductPrices(PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #endregion

        #region Insert
        //1392/11/09 تست شده
        public JsonResult ProductPrices_Insert(IEnumerable<AddProductPriceRequest> requests,Guid ProductID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("ProductPrice_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion
            response = _productPriceService.AddProductPrices(requests, GetEmployee().ID,ProductID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update
        //1392/11/09 تست شد
        public JsonResult ProductPrices_Update(IEnumerable<EditProductPriceRequest> requests,Guid ProductID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("ProductPrice_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            // ابجکت تولید شده برای تست سرویس
            //IEnumerable<EditProductPriceRequest> test = new[] { 
            //    new EditProductPriceRequest(){
            //        ID=Guid.Parse("A07F8210-CC47-4469-8D7B-327284C36BC4"),
            //        RowVersion=3,
            //        Discontinued=false,
            //        Imposition=100,
            //        MaxDiscount=1000,
            //        Note="محصول خوبیه",
            //        ProductID=Guid.Parse("8EF1BCF8-42BA-4DEA-AE15-541AFEE10781"),
            //        ProductPriceCode=503,
            //        ProductPriceTitle="نمنه",
            //        SortOrder=10,
            //        UnitPrice=50000

            //    }
            //};


            response = _productPriceService.EditProductPrices(requests, GetEmployee().ID,ProductID);

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Delete

        public JsonResult ProductPrices_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("ProductPrice_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _productPriceService.DeleteProductPrices(requests);

            return Json(response, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region Moveing

        public JsonResult ProductPrices_MoveUp(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _productPriceService.MoveUp(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProductPrices_MoveDown(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _productPriceService.MoveDown(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Private Members

        private ProductPriceView GetProductPriceView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetProductPriceResponse response = this._productPriceService.GetProductPrice(request);

            return response.ProductPriceView;
        }

        #endregion

    }
}
