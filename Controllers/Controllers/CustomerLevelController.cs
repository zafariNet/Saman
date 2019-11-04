#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.CustomerCatalog;
using System.Web.Mvc;
//using System.Web.UI.WebControls;
using Services.ViewModels.Customers;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Model.Customers;

#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class CustomerLevelController : BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly ICustomerLevelService _customerLevelService;

        #endregion

        #region Ctor

        public CustomerLevelController(IEmployeeService employeeService, 
            ICustomerLevelService customerLevelService)
            : base(employeeService)
        {
            this._customerLevelService = customerLevelService;
            this._employeeService = employeeService;
        }

        #endregion

        #region Old Methods

        public ActionResult Index()
        {
            CustomerLevelHomePageView customerLevelHomePageView = new CustomerLevelHomePageView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CustomerLevel_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerLevelHomePageView);
            }
            #endregion

            customerLevelHomePageView.EmployeeView = GetEmployee();
            customerLevelHomePageView.CustomerLevelViews = this._customerLevelService.GetCustomerLevels().CustomerLevelViews;

            return View(customerLevelHomePageView);
        }

        public ActionResult Create()
        {
            CustomerLevelDetailView customerLevelView = new CustomerLevelDetailView();
            customerLevelView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CustomerLevel_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerLevelView);
            }
            #endregion

            return View(customerLevelView);
        }

        [HttpPost]
        public ActionResult Create(CustomerLevelDetailView customerLevelDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CustomerLevel_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerLevelDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddCustomerLevelRequest request = new AddCustomerLevelRequest();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.CustomerID = customerLevelDetailView.CustomerLevelView.CustomerID;
                    request.NewLevelID = customerLevelDetailView.CustomerLevelView.LevelID;
                    request.Note = customerLevelDetailView.CustomerLevelView.Note;


                    GeneralResponse response = _customerLevelService.AddCustomerLevel(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(customerLevelDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(customerLevelDetailView);
                }

            return View(customerLevelDetailView);
        }

        public ActionResult Edit(string id)
        {
            CustomerLevelDetailView customerLevelDetailView = new CustomerLevelDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CustomerLevel_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerLevelDetailView);
            }
            #endregion

            customerLevelDetailView.CustomerLevelView = this.GetCustomerLevelView(id);
            //agencyDetailView.EmployeeView = GetEmployee();

            return View(customerLevelDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, CustomerLevelDetailView customerLevelDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CustomerLevel_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerLevelDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditCustomerLevelRequest request = new EditCustomerLevelRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.CustomerID = customerLevelDetailView.CustomerLevelView.CustomerID;
                    request.NewLevelID = customerLevelDetailView.CustomerLevelView.LevelID;
                    request.Note = customerLevelDetailView.CustomerLevelView.Note;
                    request.RowVersion = customerLevelDetailView.CustomerLevelView.RowVersion;

                    GeneralResponse response = this._customerLevelService.EditCustomerLevel(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(customerLevelDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(customerLevelDetailView);
                }

            return View(customerLevelDetailView);
        }

        public ActionResult Details(string id)
        {
            CustomerLevelDetailView customerLevelDetailView = new CustomerLevelDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CustomerLevel_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerLevelDetailView);
            }
            #endregion


            CustomerLevelView customerLevelView = this.GetCustomerLevelView(id);

            customerLevelDetailView.CustomerLevelView = customerLevelView;

            return View(customerLevelDetailView);
        }

        public ActionResult Delete(string id)
        {
            CustomerLevelDetailView customerLevelDetailView = new CustomerLevelDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CustomerLevel_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerLevelDetailView);
            }
            #endregion


            customerLevelDetailView.CustomerLevelView = this.GetCustomerLevelView(id);

            return View(customerLevelDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            CustomerLevelDetailView customerLevelDetailView = new CustomerLevelDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CustomerLevel_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(customerLevelDetailView);
            }
            #endregion

            customerLevelDetailView.CustomerLevelView = this.GetCustomerLevelView(id);
            //agencyDetailView.EmployeeView = GetEmployee();

            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._customerLevelService.DeleteCustomerLevel(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(customerLevelDetailView);
            }
        }

        #endregion


        #region Private Members

        private CustomerLevelView GetCustomerLevelView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetCustomerLevelResponse response = this._customerLevelService.GetCustomerLevel(request);

            return response.CustomerLevelView;
        }
        #endregion

    }
}

