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
    public class UncreditSaleDetailController: BaseController
    {
        private readonly IEmployeeService _employeeService;

        private readonly IUncreditSaleDetailService _uncreditSaleDetailService;

        public UncreditSaleDetailController(IEmployeeService employeeService, IUncreditSaleDetailService uncreditSaleDetailService)
            : base(employeeService)
        {
            this._uncreditSaleDetailService = uncreditSaleDetailService;
            this._employeeService = employeeService;
        }

        public ActionResult Index()
        {
            UncreditSaleDetailHomePageView uncreditSaleDetailHomePageView = new UncreditSaleDetailHomePageView();
            uncreditSaleDetailHomePageView.EmployeeView = GetEmployee();
            uncreditSaleDetailHomePageView.UncreditSaleDetailViews = this._uncreditSaleDetailService.GetUncreditSaleDetails().UncreditSaleDetailViews;

            return View(uncreditSaleDetailHomePageView);
        }

        public ActionResult Create()
        {
            UncreditSaleDetailDetailView uncreditSaleDetailDetailView = new UncreditSaleDetailDetailView();
            uncreditSaleDetailDetailView.EmployeeView = GetEmployee();

            return View(uncreditSaleDetailDetailView);
        }

        [HttpPost]
        public ActionResult Create(UncreditSaleDetailDetailView uncreditSaleDetailDetailView)
        {
            if (ModelState.IsValid)
                try
                {
                    AddUncreditSaleDetailRequest request = new AddUncreditSaleDetailRequest();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.Discount = uncreditSaleDetailDetailView.UncreditSaleDetailView.Discount;
                    request.Imposition = uncreditSaleDetailDetailView.UncreditSaleDetailView.Imposition;
                    request.LineDiscount = uncreditSaleDetailDetailView.UncreditSaleDetailView.LineDiscount;
                    request.LineImposition = uncreditSaleDetailDetailView.UncreditSaleDetailView.LineImposition;
                    request.LineTotal = uncreditSaleDetailDetailView.UncreditSaleDetailView.LineTotal;
                    request.MainUncreditSaleDetailID = uncreditSaleDetailDetailView.UncreditSaleDetailView.MainUncreditSaleDetailID;
                    request.RollbackNote = uncreditSaleDetailDetailView.UncreditSaleDetailView.RollbackNote;
                    request.SaleID = uncreditSaleDetailDetailView.UncreditSaleDetailView.SaleID;
                    request.UncreditServiceID = uncreditSaleDetailDetailView.UncreditSaleDetailView.UncreditServiceID;
                    request.UnitPrice = uncreditSaleDetailDetailView.UncreditSaleDetailView.UnitPrice;
                    request.Units = uncreditSaleDetailDetailView.UncreditSaleDetailView.Units;

                    AddResponse response = this._uncreditSaleDetailService.AddUncreditSaleDetail(request);

                    if (response.Success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(uncreditSaleDetailDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(uncreditSaleDetailDetailView);
                }

            return View(uncreditSaleDetailDetailView);
        }

        public ActionResult Edit(string id)
        {
            UncreditSaleDetailDetailView uncreditSaleDetailDetailView = new UncreditSaleDetailDetailView();
            uncreditSaleDetailDetailView.UncreditSaleDetailView = this.GetUncreditSaleDetailView(id);
            //uncreditSaleDetailDetailView.EmployeeView = GetEmployee();

            return View(uncreditSaleDetailDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, UncreditSaleDetailDetailView uncreditSaleDetailDetailView)
        {
            if (ModelState.IsValid)
                try
                {
                    EditUncreditSaleDetailRequest request = new EditUncreditSaleDetailRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.Discount = uncreditSaleDetailDetailView.UncreditSaleDetailView.Discount;
                    request.Imposition = uncreditSaleDetailDetailView.UncreditSaleDetailView.Imposition;
                    request.LineDiscount = uncreditSaleDetailDetailView.UncreditSaleDetailView.LineDiscount;
                    request.LineImposition = uncreditSaleDetailDetailView.UncreditSaleDetailView.LineImposition;
                    request.LineTotal = uncreditSaleDetailDetailView.UncreditSaleDetailView.LineTotal;
                    request.MainUncreditSaleDetailID = uncreditSaleDetailDetailView.UncreditSaleDetailView.MainUncreditSaleDetailID;
                    request.RollbackNote = uncreditSaleDetailDetailView.UncreditSaleDetailView.RollbackNote;
                    request.SaleID = uncreditSaleDetailDetailView.UncreditSaleDetailView.SaleID;
                    request.UncreditServiceID = uncreditSaleDetailDetailView.UncreditSaleDetailView.UncreditServiceID;
                    request.UnitPrice = uncreditSaleDetailDetailView.UncreditSaleDetailView.UnitPrice;
                    request.Units = uncreditSaleDetailDetailView.UncreditSaleDetailView.Units;
                    request.RowVersion = uncreditSaleDetailDetailView.UncreditSaleDetailView.RowVersion;

                    EditResponse response = this._uncreditSaleDetailService.EditUncreditSaleDetail(request);

                    if (response.Success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(uncreditSaleDetailDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(uncreditSaleDetailDetailView);
                }

            return View(uncreditSaleDetailDetailView);
        }

        public ActionResult Details(string id)
        {
            UncreditSaleDetailView uncreditSaleDetailView = this.GetUncreditSaleDetailView(id);

            UncreditSaleDetailDetailView uncreditSaleDetailDetailView = new UncreditSaleDetailDetailView();
            uncreditSaleDetailDetailView.UncreditSaleDetailView = uncreditSaleDetailView;
            // uncreditSaleDetailDetailView.EmployeeView = GetEmployee();

            return View(uncreditSaleDetailDetailView);
        }

        public ActionResult Delete(string id)
        {
            UncreditSaleDetailDetailView uncreditSaleDetailDetailView = new UncreditSaleDetailDetailView();
            uncreditSaleDetailDetailView.UncreditSaleDetailView = this.GetUncreditSaleDetailView(id);
            //uncreditSaleDetailDetailView.EmployeeView = GetEmployee();

            return View(uncreditSaleDetailDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            UncreditSaleDetailDetailView uncreditSaleDetailDetailView = new UncreditSaleDetailDetailView();
            uncreditSaleDetailDetailView.UncreditSaleDetailView = this.GetUncreditSaleDetailView(id);
            //uncreditSaleDetailDetailView.EmployeeView = GetEmployee();
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            DeleteResponse response = this._uncreditSaleDetailService.DeleteUncreditSaleDetail(request);

            if (response.Success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(uncreditSaleDetailDetailView);
            }
        }

        #region Private Members

        private UncreditSaleDetailView GetUncreditSaleDetailView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetUncreditSaleDetailResponse response = this._uncreditSaleDetailService.GetUncreditSaleDetail(request);

            return response.UncreditSaleDetailView;
        }

        #endregion

    }
}
