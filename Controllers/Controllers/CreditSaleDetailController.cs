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
    public class CreditSaleDetailController: BaseController
    {
        private readonly IEmployeeService _employeeService;

        private readonly ICreditSaleDetailService _creditSaleDetailService;

        public CreditSaleDetailController(IEmployeeService employeeService, ICreditSaleDetailService creditSaleDetailService)
            : base(employeeService)
        {
            this._creditSaleDetailService = creditSaleDetailService;
            this._employeeService = employeeService;
        }

        public ActionResult Index()
        {
            CreditSaleDetailHomePageView creditSaleDetailHomePageView = new CreditSaleDetailHomePageView();
            creditSaleDetailHomePageView.EmployeeView = GetEmployee();
            creditSaleDetailHomePageView.CreditSaleDetailViews = this._creditSaleDetailService.GetCreditSaleDetails().CreditSaleDetailViews;

            return View(creditSaleDetailHomePageView);
        }

        public ActionResult Create()
        {
            CreditSaleDetailDetailView creditSaleDetailDetailView = new CreditSaleDetailDetailView();
            creditSaleDetailDetailView.EmployeeView = GetEmployee();

            return View(creditSaleDetailDetailView);
        }

        public ActionResult Edit(string id)
        {
            CreditSaleDetailDetailView creditSaleDetailDetailView = new CreditSaleDetailDetailView();
            creditSaleDetailDetailView.CreditSaleDetailView = this.GetCreditSaleDetailView(id);
            creditSaleDetailDetailView.EmployeeView = GetEmployee();

            return View(creditSaleDetailDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, CreditSaleDetailDetailView creditSaleDetailDetailView)
        {
            if (ModelState.IsValid)
                try
                {
                    EditCreditSaleDetailRequest request = new EditCreditSaleDetailRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.CreditServiceID = creditSaleDetailDetailView.CreditSaleDetailView.CreditServiceID;
                    request.Discount = creditSaleDetailDetailView.CreditSaleDetailView.Discount;
                    request.Imposition = creditSaleDetailDetailView.CreditSaleDetailView.Imposition;
                    request.MainCreditSaleDetailID = creditSaleDetailDetailView.CreditSaleDetailView.MainCreditSaleDetailID;
                    request.UnitPrice = creditSaleDetailDetailView.CreditSaleDetailView.UnitPrice;
                    request.Units = creditSaleDetailDetailView.CreditSaleDetailView.Units;
                    request.RowVersion = creditSaleDetailDetailView.CreditSaleDetailView.RowVersion;

                    EditResponse response = this._creditSaleDetailService.EditCreditSaleDetail(request);

                    if (response.Success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(creditSaleDetailDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(creditSaleDetailDetailView);
                }

            return View(creditSaleDetailDetailView);
        }

        public ActionResult Details(string id)
        {
            CreditSaleDetailView creditSaleDetailView = this.GetCreditSaleDetailView(id);

            CreditSaleDetailDetailView creditSaleDetailDetailView = new CreditSaleDetailDetailView();
            creditSaleDetailDetailView.CreditSaleDetailView = creditSaleDetailView;
            creditSaleDetailDetailView.EmployeeView = GetEmployee();

            return View(creditSaleDetailDetailView);
        }

        public ActionResult Delete(string id)
        {
            CreditSaleDetailDetailView creditSaleDetailDetailView = new CreditSaleDetailDetailView();
            creditSaleDetailDetailView.CreditSaleDetailView = this.GetCreditSaleDetailView(id);
            //creditSaleDetailDetailView.EmployeeView = GetEmployee();

            return View(creditSaleDetailDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            CreditSaleDetailDetailView creditSaleDetailDetailView = new CreditSaleDetailDetailView();
            creditSaleDetailDetailView.CreditSaleDetailView = this.GetCreditSaleDetailView(id);
            //creditSaleDetailDetailView.EmployeeView = GetEmployee();
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            DeleteResponse response = this._creditSaleDetailService.DeleteCreditSaleDetail(request);

            if (response.Success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(creditSaleDetailDetailView);
            }
        }

        #region Private Members

        private CreditSaleDetailView GetCreditSaleDetailView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetCreditSaleDetailResponse response = this._creditSaleDetailService.GetCreditSaleDetail(request);

            return response.CreditSaleDetailView;
        }

        #endregion

    }
}
