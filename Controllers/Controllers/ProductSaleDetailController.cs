using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.SaleCatalog;
using System.Web.Mvc;
//using System.Web.UI.WebControls;
using Services.ViewModels.Sales;
using Services.Messaging;
using Services.Messaging.SaleCatalogService;

namespace Controllers.Controllers
{
    [Authorize]
    public class ProductSaleDetailController: BaseController
    {
        private readonly IEmployeeService _employeeService;

        private readonly IProductSaleDetailService _productSaleDetailService;

        public ProductSaleDetailController(IEmployeeService employeeService, IProductSaleDetailService productSaleDetailService)
            : base(employeeService)
        {
            this._productSaleDetailService = productSaleDetailService;
            this._employeeService = employeeService;
        }

        public ActionResult Index()
        {
            ProductSaleDetailHomePageView productSaleDetailHomePageView = new ProductSaleDetailHomePageView();
            productSaleDetailHomePageView.EmployeeView = GetEmployee();
            productSaleDetailHomePageView.ProductSaleDetailViews = this._productSaleDetailService.GetProductSaleDetails().ProductSaleDetailViews;

            return View(productSaleDetailHomePageView);
        }

        public ActionResult Create()
        {
            ProductSaleDetailDetailView productSaleDetailDetailView = new ProductSaleDetailDetailView();
            productSaleDetailDetailView.EmployeeView = GetEmployee();

            return View(productSaleDetailDetailView);
        }

        [HttpPost]
        public ActionResult Create(ProductSaleDetailDetailView productSaleDetailDetailView)
        {
            if (ModelState.IsValid)
                try
                {
                    AddProductSaleDetailRequest request = new AddProductSaleDetailRequest();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.Discount = productSaleDetailDetailView.ProductSaleDetailView.Discount;
                    request.Imposition = productSaleDetailDetailView.ProductSaleDetailView.Imposition;
                    request.LineDiscount = productSaleDetailDetailView.ProductSaleDetailView.LineDiscount;
                    request.LineImposition = productSaleDetailDetailView.ProductSaleDetailView.LineImposition;
                    request.MainProductSaleDetailID = productSaleDetailDetailView.ProductSaleDetailView.MainProductSaleDetailID;
                    request.ProductPriceID = productSaleDetailDetailView.ProductSaleDetailView.ProductID;
                    request.RollbackNote = productSaleDetailDetailView.ProductSaleDetailView.RollbackNote;
                    request.SaleID = productSaleDetailDetailView.ProductSaleDetailView.SaleID;
                    request.UnitPrice = productSaleDetailDetailView.ProductSaleDetailView.UnitPrice;
                    request.Units = productSaleDetailDetailView.ProductSaleDetailView.Units;

                    AddResponse response = this._productSaleDetailService.AddProductSaleDetail(request);

                    if (response.Success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(productSaleDetailDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(productSaleDetailDetailView);
                }

            return View(productSaleDetailDetailView);
        }

        public ActionResult Edit(string id)
        {
            ProductSaleDetailDetailView productSaleDetailDetailView = new ProductSaleDetailDetailView();
            productSaleDetailDetailView.ProductSaleDetailView = this.GetProductSaleDetailView(id);
            //productSaleDetailDetailView.EmployeeView = GetEmployee();

            return View(productSaleDetailDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, ProductSaleDetailDetailView productSaleDetailDetailView)
        {
            if (ModelState.IsValid)
                try
                {
                    EditProductSaleDetailRequest request = new EditProductSaleDetailRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.Discount = productSaleDetailDetailView.ProductSaleDetailView.Discount;
                    request.Imposition = productSaleDetailDetailView.ProductSaleDetailView.Imposition;
                    request.LineDiscount = productSaleDetailDetailView.ProductSaleDetailView.LineDiscount;
                    request.LineImposition = productSaleDetailDetailView.ProductSaleDetailView.LineImposition;
                    request.MainProductSaleDetailID = productSaleDetailDetailView.ProductSaleDetailView.MainProductSaleDetailID;
                    request.ProductPriceID = productSaleDetailDetailView.ProductSaleDetailView.ProductID;
                    request.RollbackNote = productSaleDetailDetailView.ProductSaleDetailView.RollbackNote;
                    request.SaleID = productSaleDetailDetailView.ProductSaleDetailView.SaleID;
                    request.UnitPrice = productSaleDetailDetailView.ProductSaleDetailView.UnitPrice;
                    request.Units = productSaleDetailDetailView.ProductSaleDetailView.Units;
                    request.RowVersion = productSaleDetailDetailView.ProductSaleDetailView.RowVersion;

                    EditResponse response = this._productSaleDetailService.EditProductSaleDetail(request);

                    if (response.Success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(productSaleDetailDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(productSaleDetailDetailView);
                }

            return View(productSaleDetailDetailView);
        }

        public ActionResult Details(string id)
        {
            ProductSaleDetailView productSaleDetailView = this.GetProductSaleDetailView(id);

            ProductSaleDetailDetailView productSaleDetailDetailView = new ProductSaleDetailDetailView();
            productSaleDetailDetailView.ProductSaleDetailView = productSaleDetailView;
            // productSaleDetailDetailView.EmployeeView = GetEmployee();

            return View(productSaleDetailDetailView);
        }

        public ActionResult Delete(string id)
        {
            ProductSaleDetailDetailView productSaleDetailDetailView = new ProductSaleDetailDetailView();
            productSaleDetailDetailView.ProductSaleDetailView = this.GetProductSaleDetailView(id);
            //productSaleDetailDetailView.EmployeeView = GetEmployee();

            return View(productSaleDetailDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            ProductSaleDetailDetailView productSaleDetailDetailView = new ProductSaleDetailDetailView();
            productSaleDetailDetailView.ProductSaleDetailView = this.GetProductSaleDetailView(id);
            //productSaleDetailDetailView.EmployeeView = GetEmployee();
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            DeleteResponse response = this._productSaleDetailService.DeleteProductSaleDetail(request);

            if (response.Success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(productSaleDetailDetailView);
            }
        }

        #region Private Members

        private ProductSaleDetailView GetProductSaleDetailView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetProductSaleDetailResponse response = this._productSaleDetailService.GetProductSaleDetail(request);

            return response.ProductSaleDetailView;
        }

        #endregion

    }
}
