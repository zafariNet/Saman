#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.CustomerCatalog;
using System.Web.Mvc;
using Services.ViewModels.Customers;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class QueryEmployeeController: BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly IQueryEmployeeService _queryEmployeeService;

        #endregion

        #region Ctor

        public QueryEmployeeController(IEmployeeService employeeService, IQueryEmployeeService queryEmployeeService)
            : base(employeeService)
        {
            this._queryEmployeeService = queryEmployeeService;
            this._employeeService = employeeService;
        }

        #endregion

        #region Index

        public ActionResult Index(string id)
        {
            QueryEmployeeHomePageView queryEmployeeHomePageView = new QueryEmployeeHomePageView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("QueryEmployee_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryEmployeeHomePageView);
            }
            #endregion

            queryEmployeeHomePageView.EmployeeView = GetEmployee();
            queryEmployeeHomePageView.QueryEmployeeViews = _queryEmployeeService.GetQueryEmployees(Guid.Parse(id)).QueryEmployeeViews;

            return View(queryEmployeeHomePageView);
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            QueryEmployeeDetailView queryEmployeeDetailView = new QueryEmployeeDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("QueryEmployee_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryEmployeeDetailView);
            }
            #endregion
            
            queryEmployeeDetailView.EmployeeView = GetEmployee();

            return View(queryEmployeeDetailView);
        }

        [HttpPost]
        public ActionResult Create(QueryEmployeeDetailView queryEmployeeDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("QueryEmployee_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryEmployeeDetailView);
            }
            #endregion
            

            queryEmployeeDetailView.EmployeeView = GetEmployee();
            
            if (ModelState.IsValid)
                try
                {
                    AddQueryEmployeeRequestOld request = new AddQueryEmployeeRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    //request.EmployeeID = queryEmployeeDetailView.QueryEmployeeView.EmployeeID;
                    //request.QueryID = queryEmployeeDetailView.QueryEmployeeView.QueryID;

                    GeneralResponse response = this._queryEmployeeService.AddQueryEmployee(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(queryEmployeeDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(queryEmployeeDetailView);
                }

            return View(queryEmployeeDetailView);
        }

        #endregion

        #region Edit

        public ActionResult Edit(string id)
        {
            QueryEmployeeDetailView queryEmployeeDetailView = new QueryEmployeeDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("QueryEmployee_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryEmployeeDetailView);
            }
            #endregion
            

            //queryEmployeeDetailView.QueryEmployeeView = this.GetQueryEmployeeView(id);
            queryEmployeeDetailView.EmployeeView = GetEmployee();

            return View(queryEmployeeDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, QueryEmployeeDetailView queryEmployeeDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("QueryEmployee_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryEmployeeDetailView);
            }
            #endregion

            queryEmployeeDetailView.EmployeeView = GetEmployee();
            
            if (ModelState.IsValid)
                try
                {
                    EditQueryEmployeeRequest request = new EditQueryEmployeeRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    //request.EmployeeID = queryEmployeeDetailView.QueryEmployeeView.EmployeeID;
                    //request.QueryID = queryEmployeeDetailView.QueryEmployeeView.QueryID;
                    //request.RowVersion = queryEmployeeDetailView.QueryEmployeeView.RowVersion;

                    GeneralResponse response = this._queryEmployeeService.EditQueryEmployee(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(queryEmployeeDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(queryEmployeeDetailView);
                }

            return View(queryEmployeeDetailView);
        }

        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            QueryEmployeeDetailView queryEmployeeDetailView = new QueryEmployeeDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("QueryEmployee_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryEmployeeDetailView);
            }
            #endregion

            QueryEmployeeView queryEmployeeView = this.GetQueryEmployeeView(id);

            //queryEmployeeDetailView.QueryEmployeeView = queryEmployeeView;
            queryEmployeeDetailView.EmployeeView = GetEmployee();

            return View(queryEmployeeDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            QueryEmployeeDetailView queryEmployeeDetailView = new QueryEmployeeDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("QueryEmployee_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryEmployeeDetailView);
            }
            #endregion

            
            //queryEmployeeDetailView.QueryEmployeeView = this.GetQueryEmployeeView(id);
            queryEmployeeDetailView.EmployeeView = GetEmployee();

            return View(queryEmployeeDetailView);
        }

        #endregion

        #region Private Members

        private QueryEmployeeView GetQueryEmployeeView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetQueryEmployeeResponse response = this._queryEmployeeService.GetQueryEmployee(request);

            return response.QueryEmployeeView;
        }

        #endregion

    }
}
