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
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class ConditionController : BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly IConditionService _conditionService;
        #endregion

        #region Ctor
        public ConditionController(IEmployeeService employeeService, IConditionService conditionService)
            : base(employeeService)
        {
            this._conditionService = conditionService;
            this._employeeService = employeeService;
        }
        #endregion

        #region Old Methods

        #region Ajax Read
        public ActionResult Condition_Read([DataSourceRequest] DataSourceRequest request)
        {
            ConditionHomePageView conditionHomePageView = new ConditionHomePageView();
            conditionHomePageView.EmployeeView = GetEmployee();
            GetGeneralResponse<IEnumerable<ConditionView>> conditionResponse=new GetGeneralResponse<IEnumerable<ConditionView>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                var _result = new DataSourceResult()
                {
                    Data = conditionResponse.data,
                    Total = conditionResponse.totalCount
                };
                return Json(_result);
            }
            #endregion

            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            getRequest.PageNumber = request.Page;
            conditionResponse = this._conditionService.GetConditions(getRequest);

            conditionHomePageView.ConditionViews = conditionResponse.data;
            conditionHomePageView.Count = conditionResponse.totalCount;

            var result = new DataSourceResult()
            {
                Data = conditionResponse.data,
                Total = conditionResponse.totalCount
            };
            return Json(result);
        }
        #endregion

        #region Index
        public ActionResult Index()
        {
            ConditionHomePageView conditionHomePageView = new ConditionHomePageView();
            conditionHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(conditionHomePageView);
            }
            #endregion

            conditionHomePageView.ConditionViews = this._conditionService.GetConditions().ConditionViews;

            return View(conditionHomePageView);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            ConditionDetailView conditionDetailView = new ConditionDetailView();
            conditionDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(conditionDetailView);
            }
            #endregion

            return View(conditionDetailView);
        }

        [HttpPost]
        public ActionResult Create(ConditionDetailView conditionDetailView)
        {
            conditionDetailView.EmployeeView = GetEmployee();
            
            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(conditionDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddConditionRequestOld request = new AddConditionRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.ConditionTitle = conditionDetailView.ConditionView.ConditionTitle;
                    //request.CriteriaOperator = conditionDetailView.ConditionView.CriteriaOperator;
                    request.ErrorText = conditionDetailView.ConditionView.ErrorText;
                    request.nHibernate = conditionDetailView.ConditionView.nHibernate;
                    //request.PropertyName = conditionDetailView.ConditionView.PropertyName;
                    request.QueryText = conditionDetailView.ConditionView.QueryText;
                    //request.Value = conditionDetailView.ConditionView.Value;


                    GeneralResponse response = this._conditionService.AddCondition(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(conditionDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(conditionDetailView);
                }

            return View(conditionDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            ConditionDetailView conditionDetailView = new ConditionDetailView();
            conditionDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(conditionDetailView);
            }
            #endregion

            conditionDetailView.ConditionView = this.GetConditionView(id);
            
            return View(conditionDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, ConditionDetailView conditionDetailView)
        {
            conditionDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(conditionDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditConditionRequestOld request = new EditConditionRequestOld();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.ConditionTitle = conditionDetailView.ConditionView.ConditionTitle;
                    //request.CriteriaOperator = conditionDetailView.ConditionView.CriteriaOperator;
                    request.ErrorText = conditionDetailView.ConditionView.ErrorText;
                    request.nHibernate = conditionDetailView.ConditionView.nHibernate;
                    //request.PropertyName = conditionDetailView.ConditionView.PropertyName;
                    request.QueryText = conditionDetailView.ConditionView.QueryText;
                    //request.Value = conditionDetailView.ConditionView.Value;
                    request.RowVersion = conditionDetailView.ConditionView.RowVersion;

                    GeneralResponse response = this._conditionService.EditCondition(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(conditionDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(conditionDetailView);
                }

            return View(conditionDetailView);
        }

        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            ConditionDetailView conditionDetailView = new ConditionDetailView();
            conditionDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(conditionDetailView);
            }
            #endregion

            ConditionView conditionView = this.GetConditionView(id);
            conditionDetailView.ConditionView = conditionView;
            

            return View(conditionDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            ConditionDetailView conditionDetailView = new ConditionDetailView();
            conditionDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(conditionDetailView);
            }
            #endregion

            conditionDetailView.ConditionView = this.GetConditionView(id);
            
            return View(conditionDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            ConditionDetailView conditionDetailView = new ConditionDetailView();
            conditionDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(conditionDetailView);
            }
            #endregion

            conditionDetailView.ConditionView = this.GetConditionView(id);
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._conditionService.DeleteCondition(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(conditionDetailView);
            }
        }
        #endregion

        #endregion

        #region New Methods

        #region Read

        public JsonResult Conditions_Read(int? pageSize, int? pageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<ConditionView>> response = new GetGeneralResponse<IEnumerable<ConditionView>>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _conditionService.GetConditions(PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Insert

        public JsonResult Conditions_Insert(IEnumerable<AddConditionRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _conditionService.AddConditions(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult Conditions_Update(IEnumerable<EditConditionRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _conditionService.EditConditions(requests, GetEmployee().ID);

            return Json(response,JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Delete

        public JsonResult Conditions_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _conditionService.DeleteConditions(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Private Members

        private ConditionView GetConditionView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetConditionResponse response = this._conditionService.GetCondition(request);

            return response.ConditionView;
        }

        #endregion

        
    }
}
