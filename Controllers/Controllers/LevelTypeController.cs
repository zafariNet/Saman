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
    public class LevelTypeController : BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly ILevelTypeService _levelTypeService;
        #endregion

        #region Ctor
        public LevelTypeController(IEmployeeService employeeService, ILevelTypeService levelTypeService)
            : base(employeeService)
        {
            this._levelTypeService = levelTypeService;
            this._employeeService = employeeService;
        }
        #endregion

        #region Old Methods
        
        #region Ajax Read
        public ActionResult LevelType_Read([DataSourceRequest] DataSourceRequest request)
        {
            LevelTypeHomePageView levelTypeHomePageView = new LevelTypeHomePageView();
            levelTypeHomePageView.EmployeeView = GetEmployee();
            GetGeneralResponse<IEnumerable<LevelTypeView>> levelTypeResponse=new GetGeneralResponse<IEnumerable<LevelTypeView>>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Read");
            if (!hasPermission)
            {
                
            var _result = new DataSourceResult()
            {
                Data = levelTypeResponse.data,
                Total = levelTypeResponse.totalCount
            };
                ModelState.AddModelError("","AccessDenied");
                return Json(_result);
            }

            #endregion

            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            getRequest.PageNumber = request.Page;
            levelTypeResponse = this._levelTypeService.GetLevelTypes(getRequest);

            levelTypeHomePageView.LevelTypeViews = levelTypeResponse.data;
            levelTypeHomePageView.Count = levelTypeResponse.totalCount;

            var result = new DataSourceResult()
            {
                Data = levelTypeResponse.data,
                Total = levelTypeResponse.totalCount
            };
            return Json(result);
        }
        #endregion

        #region Index
        public ActionResult Index()
        {
            LevelTypeHomePageView levelTypeHomePageView = new LevelTypeHomePageView();
            levelTypeHomePageView.EmployeeView = GetEmployee();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Read");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(levelTypeHomePageView);
            }

            #endregion

            levelTypeHomePageView.LevelTypeViews = this._levelTypeService.GetLevelTypes().LevelTypeViews;

            return View(levelTypeHomePageView);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            LevelTypeDetailView levelTypeDetailView = new LevelTypeDetailView();
            levelTypeDetailView.EmployeeView = GetEmployee();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Insert");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(levelTypeDetailView);
            }

            #endregion

            return View(levelTypeDetailView);
        }

        [HttpPost]
        public ActionResult Create(LevelTypeDetailView levelTypeDetailView)
        {
            levelTypeDetailView.EmployeeView = GetEmployee();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Insert");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(levelTypeDetailView);
            }

            #endregion


            if (ModelState.IsValid)
                try
                {
                    AddLevelTypeRequestOld request = new AddLevelTypeRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.Title = levelTypeDetailView.LevelTypeView.Title;

                    GeneralResponse response = this._levelTypeService.AddLevelType(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(levelTypeDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(levelTypeDetailView);
                }

            return View(levelTypeDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            LevelTypeDetailView levelTypeDetailView = new LevelTypeDetailView();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Insert");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(levelTypeDetailView);
            }

            #endregion

            levelTypeDetailView.LevelTypeView = this.GetLevelTypeView(id);
            levelTypeDetailView.EmployeeView = GetEmployee();

            return View(levelTypeDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, LevelTypeDetailView levelTypeDetailView)
        {
            levelTypeDetailView.EmployeeView = GetEmployee();


            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Insert");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(levelTypeDetailView);
            }

            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditLevelTypeRequestOld request = new EditLevelTypeRequestOld();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.Title = levelTypeDetailView.LevelTypeView.Title;
                    request.RowVersion = levelTypeDetailView.LevelTypeView.RowVersion;

                    GeneralResponse response = this._levelTypeService.EditLevelType(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(levelTypeDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(levelTypeDetailView);
                }

            return View(levelTypeDetailView);
        }
        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            LevelTypeDetailView levelTypeDetailView = new LevelTypeDetailView();
            levelTypeDetailView.EmployeeView = GetEmployee();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Read");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(levelTypeDetailView);
            }

            #endregion

            LevelTypeView levelTypeView = this.GetLevelTypeView(id);
            
            levelTypeDetailView.LevelTypeView = levelTypeView;
            

            return View(levelTypeDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            LevelTypeDetailView levelTypeDetailView = new LevelTypeDetailView();
            levelTypeDetailView.EmployeeView = GetEmployee();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Read");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(levelTypeDetailView);
            }

            #endregion

            levelTypeDetailView.LevelTypeView = this.GetLevelTypeView(id);
            
            return View(levelTypeDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            LevelTypeDetailView levelTypeDetailView = new LevelTypeDetailView();
            levelTypeDetailView.EmployeeView = GetEmployee();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Delete");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return View(levelTypeDetailView);
            }

            #endregion

            
            levelTypeDetailView.LevelTypeView = this.GetLevelTypeView(id);
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._levelTypeService.DeleteLevelType(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(levelTypeDetailView);
            }
        }
        #endregion

        #region Private Members

        private LevelTypeView GetLevelTypeView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetLevelTypeResponse response = this._levelTypeService.GetLevelType(request);

            return response.LevelTypeView;
        }

        #endregion
        
        #endregion

        #region New Methods

        #region Read

        public JsonResult LevelTypes_Read(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<LevelTypeView>> levelTypeResponse=new GetGeneralResponse<IEnumerable<LevelTypeView>>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Read");
            if (!hasPermission)
            {

                ModelState.AddModelError("", "AccessDenied");
                return Json(levelTypeResponse);
            }

            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            levelTypeResponse = this._levelTypeService.GetLevelTypes(PageSize, PageNumber);


            return Json(levelTypeResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult LevelTypes_Insert(IEnumerable<AddLevelTypeRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Insert");
            if (!hasPermission)
            {

                response.ErrorMessages.Add("AccessDenied");
                return Json(response,JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _levelTypeService.AddLevelTypes(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult LevelTypes_Edit(IEnumerable<EditLevelTypeRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Update");
            if (!hasPermission)
            {

                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _levelTypeService.EditleveTypes(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete
        public JsonResult LevelTypes_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Delete");
            if (!hasPermission)
            {

                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _levelTypeService.DeleteLeveleTypes(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
    }
}
