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
using Kendo.Mvc.UI;
using Controllers.ViewModels;
using Services.ViewModels.Employees;
using Services.ViewModels;
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class QueryController: BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly IQueryService _queryService;

        private readonly IQueryEmployeeService _queryEmployeeService;
        #endregion

        #region Ctor
        public QueryController(IEmployeeService employeeService, IQueryService queryService
            , IQueryEmployeeService queryEmployeeService)
            : base(employeeService)
        {
            _queryService = queryService;
            _employeeService = employeeService;
            _queryEmployeeService = queryEmployeeService;
        }
        #endregion

        #region Old Methods

        #region Ajax Read

        public ActionResult Query_Read([DataSourceRequest] DataSourceRequest request)
        {
            GetQueriesResponse queryResponse=new GetQueriesResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Query_Read");
            if (!hasPermission)
            {
                var _result = new DataSourceResult()
                {
                    Data = queryResponse.QueryViews,
                    Total = queryResponse.TotalCount
                };
                ModelState.AddModelError("", "AccessDenied");
                return Json(_result);
            }
            #endregion

            QueryHomePageView queryHomePageView = new QueryHomePageView();
            queryHomePageView.EmployeeView = GetEmployee();
            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            getRequest.PageNumber = request.Page;
            queryResponse = this._queryService.GetQueries(getRequest);

            queryHomePageView.QueryViews = queryResponse.QueryViews;
            queryHomePageView.Count = queryResponse.TotalCount;

            var result = new DataSourceResult()
            {
                Data = queryResponse.QueryViews,
                Total = queryResponse.TotalCount
            };
            return Json(result);
        }

        #endregion

        #region Index

        public ActionResult Index()
        {
            QueryHomePageView queryHomePageView = new QueryHomePageView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Query_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryHomePageView);
            }
            #endregion

            queryHomePageView.EmployeeView = GetEmployee();
            queryHomePageView.QueryViews = this._queryService.GetQueries(new AjaxGetRequest() { PageNumber = 1, PageSize = 10 }).QueryViews;

            return View(queryHomePageView);
        }

        #endregion

        #region Create
        public ActionResult Create()
        {
            QueryDetailView queryDetailView = new QueryDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Query_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryDetailView);
            }
            #endregion
            
            queryDetailView.EmployeeView = GetEmployee();

            return View(queryDetailView);
        }

        [HttpPost]
        public ActionResult Create(QueryDetailView queryDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Query_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryDetailView);
            }
            #endregion

            queryDetailView.EmployeeView = GetEmployee();
            
            if (ModelState.IsValid)
                try
                {
                    AddQueryRequestOld request = new AddQueryRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.PrmDefinition = queryDetailView.QueryView.PrmDefinition;
                    request.QueryText = queryDetailView.QueryView.QueryText;
                    request.PrmValues = queryDetailView.QueryView.PrmValues;
                    request.Title = queryDetailView.QueryView.Title;
                    request.xType = "customercontainer";

                    GeneralResponse response = this._queryService.AddQuery(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(queryDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(queryDetailView);
                }

            return View(queryDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            QueryDetailView queryDetailView = new QueryDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Query_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryDetailView);
            }
            #endregion
            
            queryDetailView.QueryView = this.GetQueryView(id);
            queryDetailView.EmployeeView = GetEmployee();


            return View(queryDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, QueryDetailView queryDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Query_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryDetailView);
            }
            #endregion

            queryDetailView.EmployeeView = GetEmployee();

            if (ModelState.IsValid)
                try
                {
                    EditQueryRequestOld request = new EditQueryRequestOld();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.PrmDefinition = queryDetailView.QueryView.PrmDefinition;
                    request.QueryText = queryDetailView.QueryView.QueryText;
                    request.PrmValues = queryDetailView.QueryView.PrmValues;
                    request.Title = queryDetailView.QueryView.Title;
                    request.RowVersion = queryDetailView.QueryView.RowVersion;

                    GeneralResponse response = this._queryService.EditQuery(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(queryDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(queryDetailView);
                }

            return View(queryDetailView);
        }
        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            QueryDetailView queryDetailView = new QueryDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Query_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryDetailView);
            }
            #endregion

            QueryView queryView = this.GetQueryView(id);

            queryDetailView.QueryView = queryView;
            queryDetailView.EmployeeView = GetEmployee();

            return View(queryDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            QueryDetailView queryDetailView = new QueryDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Query_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryDetailView);
            }
            #endregion

            queryDetailView.QueryView = this.GetQueryView(id);
            queryDetailView.EmployeeView = GetEmployee();


            return View(queryDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            QueryDetailView queryDetailView = new QueryDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Query_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryDetailView);
            }
            #endregion

            queryDetailView.QueryView = this.GetQueryView(id);
            queryDetailView.EmployeeView = GetEmployee();


            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._queryService.DeleteQuery(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(queryDetailView);
            }
        }
        #endregion

        protected JsonResult CustomerQueryLinks_Read()
        {
            IList<MainMenuView> mainMenuView = new List<MainMenuView>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Query_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(mainMenuView, JsonRequestBehavior.AllowGet);
            }
            #endregion


            GetQueriesResponse queriesResponse = this._queryService.GetQueries(new GetQueriesRequest() { EmployeeID = GetEmployee().ID });

            if (queriesResponse != null && queriesResponse.QueryViews != null)
            foreach (var query in queriesResponse.QueryViews)
            {
                mainMenuView.Add(new MainMenuView()
                {
                    SubmenuName = query.Title,
                    SubmenuUrl = "Customer/Query/" + query.ID.ToString(),
                    xType = "",
                    Icon = "",
                    ID = query.ID
                });
            }

            return Json(mainMenuView, JsonRequestBehavior.AllowGet);
        }

        #region Query_Employee

        public ActionResult QueryEmployee(string id)
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

            
            queryEmployeeDetailView.EmployeeView = GetEmployee();
            queryEmployeeDetailView.QueryEmployeeViews = _queryEmployeeService.GetQueryEmployees(Guid.Parse(id)).QueryEmployeeViews;
            queryEmployeeDetailView.QueryView = GetQueryView(id);// _queryService.GetQuery(new GetRequest() { ID = Guid.Parse(id) }).QueryView;

            #region DropDownList For Employees

            queryEmployeeDetailView.EmployeeViews = _employeeService.GetEmployees().EmployeeViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (queryEmployeeDetailView.EmployeeViews.Count() > 0)
                foreach (EmployeeView employeeView in queryEmployeeDetailView.EmployeeViews)
                {
                    list.Add(new DropDownItem { Value = employeeView.ID.ToString(), Text = employeeView.Name });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["Employees"] = selectList;

            #endregion

            return View(queryEmployeeDetailView);
        }

        // Create
        [HttpPost]
        public ActionResult QueryEmployee(string id, QueryEmployeeDetailView queryEmployeeDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("QueryEmployee_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(queryEmployeeDetailView);
            }
            #endregion

            queryEmployeeDetailView.EmployeeView = GetEmployee();
            queryEmployeeDetailView.QueryEmployeeViews = _queryEmployeeService.GetQueryEmployees(Guid.Parse(id)).QueryEmployeeViews;
            queryEmployeeDetailView.QueryView = _queryService.GetQuery(new GetRequest() { ID = Guid.Parse(id) }).QueryView;

            #region DropDownList For Employees

            queryEmployeeDetailView.EmployeeViews = _employeeService.GetEmployees().EmployeeViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (queryEmployeeDetailView.EmployeeViews.Count() > 0)
                foreach (EmployeeView employeeView in queryEmployeeDetailView.EmployeeViews)
                {
                    list.Add(new DropDownItem { Value = employeeView.ID.ToString(), Text = employeeView.Name });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["Employees"] = selectList;

            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddQueryEmployeeRequestOld request = new AddQueryEmployeeRequestOld();

                    request.QueryID = Guid.Parse(id);
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.EmployeeID = queryEmployeeDetailView.EmployeeViewForInsert.ID;

                    GeneralResponse response = _queryEmployeeService.AddQueryEmployee(request);

                    if (!response.success)
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

            // Reload content of grid:
            queryEmployeeDetailView.QueryEmployeeViews = _queryEmployeeService.GetQueryEmployees(Guid.Parse(id)).QueryEmployeeViews;

            return View(queryEmployeeDetailView);
        }

        // Delete
        [HttpPost]
        public ActionResult QueryEmployee_Delete(QueryEmployeeView queryEmployeeView)
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

            GeneralResponse response = this._queryEmployeeService.DeleteQueryEmployee(queryEmployeeView.QueryID, queryEmployeeView.EmployeeID);

            if (!response.success)
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
            }
            return View(queryEmployeeDetailView);
        }

        #endregion

        #endregion

        #region New Methods

        #region Read

        #region Read One
        // تست شد 1392/11/05 
        public JsonResult New_Query_Read(Guid QueryID)
        {
            GetGeneralResponse<QueryView> response = new GetGeneralResponse<QueryView>();

            response = _queryService.GetQuery(QueryID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read All
        // تست شد 1392/11/05 
        public JsonResult New_Queries_Read(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<QueryView>> response = new GetGeneralResponse<IEnumerable<QueryView>>();

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _queryService.GetQueries(PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Insert
        // تست شد 1392/11/05 
        public JsonResult New_Query_Insert(AddQueryRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            response = _queryService.AddQuery(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Update
        // تست شد 1392/11/05 
        public JsonResult New_Query_Update(EditQueryRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            response = _queryService.EditQuery(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete
        // تست شد 1392/11/05 
        public JsonResult New_Query_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            response = _queryService.DeleteQuery(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Query Employee

        #region Read
        // تست شد 1392/11/05 
        public JsonResult QueryEmployees_Read(Guid QueryID , int? pageSize , int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<QueryEmployeeView>> response = new GetGeneralResponse<IEnumerable<QueryEmployeeView>>();
            
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _queryEmployeeService.GetQueryEmployees(QueryID, PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert
        // تست شد 1392/11/05 
        public JsonResult QueryEmployees_Insert(Guid QueryID,IEnumerable<AddQueryEmployeeRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            response = _queryEmployeeService.AddQueryEmployees(QueryID,requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete
        // تست شد 1392/11/05 
        public JsonResult QueryEmployees_Delete(IEnumerable<QueryEmployeeDeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            response = _queryEmployeeService.DeleteQueryEmployees(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion



        #endregion

        #region Private Members

        private QueryView GetQueryView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetQueryResponse response = this._queryService.GetQuery(request);

            return response.QueryView;
        }

        #endregion

    }
}
