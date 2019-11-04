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

namespace Controllers.Controllers
{
    [Authorize]
    public class ProductCategoryController: BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly IProductCategoryService _productCategoryService;

        #endregion

        #region Ctor

        public ProductCategoryController(IEmployeeService employeeService, IProductCategoryService productCategoryService)
            : base(employeeService)
        {
            this._productCategoryService = productCategoryService;
            this._employeeService = employeeService;
        }

        #endregion

        #region Old Methods

        public ActionResult Index()
        {
            ProductCategoryHomePageView productCategoryHomePageView = new ProductCategoryHomePageView();
            productCategoryHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductCategory_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return View(productCategoryHomePageView);
            }
            #endregion

            productCategoryHomePageView.ProductCategoryViews = this._productCategoryService.GetProductCategorys().ProductCategoryViews;

            return View(productCategoryHomePageView);
        }

        public ActionResult Create()
        {
            ProductCategoryDetailView productCategoryDetailView = new ProductCategoryDetailView();
            productCategoryDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductCategory_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productCategoryDetailView);
            }
            #endregion

            return View(productCategoryDetailView);
        }

        [HttpPost]
        public ActionResult Create(ProductCategoryDetailView productCategoryDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductCategory_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productCategoryDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddProductCategoryRequest request = new AddProductCategoryRequest();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.Discontinued = productCategoryDetailView.ProductCategoryView.Discontinued;
                    request.ProductCategoryName = productCategoryDetailView.ProductCategoryView.ProductCategoryName;
                    request.Note = productCategoryDetailView.ProductCategoryView.Note;

                    GeneralResponse response = this._productCategoryService.AddProductCategory(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(productCategoryDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(productCategoryDetailView);
                }

            return View(productCategoryDetailView);
        }

        public ActionResult Edit(string id)
        {
            ProductCategoryDetailView productCategoryDetailView = new ProductCategoryDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductCategory_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productCategoryDetailView);
            }
            #endregion
            
            productCategoryDetailView.ProductCategoryView = this.GetProductCategoryView(id);
            //productCategoryDetailView.EmployeeView = GetEmployee();

            return View(productCategoryDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, ProductCategoryDetailView productCategoryDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductCategory_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productCategoryDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditProductCategoryRequest request = new EditProductCategoryRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.Discontinued = productCategoryDetailView.ProductCategoryView.Discontinued;
                    request.ProductCategoryName = productCategoryDetailView.ProductCategoryView.ProductCategoryName;
                    request.Note = productCategoryDetailView.ProductCategoryView.Note;
                    request.RowVersion = productCategoryDetailView.ProductCategoryView.RowVersion;

                    GeneralResponse response = this._productCategoryService.EditProductCategory(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(productCategoryDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(productCategoryDetailView);
                }

            return View(productCategoryDetailView);
        }

        public ActionResult Details(string id)
        {
            ProductCategoryDetailView productCategoryDetailView = new ProductCategoryDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductCategory_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productCategoryDetailView);
            }
            #endregion

            ProductCategoryView productCategoryView = this.GetProductCategoryView(id);

            
            productCategoryDetailView.ProductCategoryView = productCategoryView;
            // productCategoryDetailView.EmployeeView = GetEmployee();

            return View(productCategoryDetailView);
        }

        public ActionResult Delete(string id)
        {
            ProductCategoryDetailView productCategoryDetailView = new ProductCategoryDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductCategory_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productCategoryDetailView);
            }
            #endregion

            
            productCategoryDetailView.ProductCategoryView = this.GetProductCategoryView(id);
            //productCategoryDetailView.EmployeeView = GetEmployee();

            return View(productCategoryDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            ProductCategoryDetailView productCategoryDetailView = new ProductCategoryDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ProductCategory_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(productCategoryDetailView);
            }
            #endregion
            
            productCategoryDetailView.ProductCategoryView = this.GetProductCategoryView(id);
            //productCategoryDetailView.EmployeeView = GetEmployee();
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._productCategoryService.DeleteProductCategory(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(productCategoryDetailView);
            }
        }

#endregion

        #region Private Members

        private ProductCategoryView GetProductCategoryView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetProductCategoryResponse response = this._productCategoryService.GetProductCategory(request);

            return response.ProductCategoryView;
        }

        #endregion

    }
}

