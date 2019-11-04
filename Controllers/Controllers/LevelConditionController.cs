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

#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class LevelConditionController : BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly ILevelConditionService _levelConditionService;

        #endregion

        #region Ctor

        public LevelConditionController(IEmployeeService employeeService, ILevelConditionService levelConditionService)
            : base(employeeService)
        {
            this._levelConditionService = levelConditionService;
            this._employeeService = employeeService;
        }

        #endregion

        #region Old Methods

        public ActionResult Index()
        {
            LevelConditionHomePageView levelConditionHomePageView = new LevelConditionHomePageView();
            levelConditionHomePageView.EmployeeView = GetEmployee();
            
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("LevelCondition_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(levelConditionHomePageView);
            }
            #endregion

            levelConditionHomePageView.LevelConditionViews = this._levelConditionService.GetLevelConditions().LevelConditionViews;

            return View(levelConditionHomePageView);
        }

        public ActionResult Create()
        {
            LevelConditionDetailView levelConditionDetailView = new LevelConditionDetailView();
            levelConditionDetailView.EmployeeView = GetEmployee();

            return View(levelConditionDetailView);
        }

        [HttpPost]
        public ActionResult Create(LevelConditionDetailView levelConditionDetailView)
        {
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("LevelCondition_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(levelConditionDetailView);
            }
            #endregion


            if (ModelState.IsValid)
                try
                {
                    AddLevelConditionRequest request = new AddLevelConditionRequest();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.ConditionID = levelConditionDetailView.LevelConditionView.Condition.ID;
                    request.LevelID = levelConditionDetailView.LevelConditionView.LevelID;

                    GeneralResponse response = this._levelConditionService.AddLevelCondition(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(levelConditionDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(levelConditionDetailView);
                }

            return View(levelConditionDetailView);
        }

        public ActionResult Edit(string id)
        {
            LevelConditionDetailView levelConditionDetailView = new LevelConditionDetailView();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("LevelCondition_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(levelConditionDetailView);
            }
            #endregion

            levelConditionDetailView.LevelConditionView = this.GetLevelConditionView(id);
            //levelConditionDetailView.EmployeeView = GetEmployee();

            return View(levelConditionDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, LevelConditionDetailView levelConditionDetailView)
        {

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("LevelCondition_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(levelConditionDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditLevelConditionRequest request = new EditLevelConditionRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.ConditionID = levelConditionDetailView.LevelConditionView.Condition.ID;
                    request.LevelID = levelConditionDetailView.LevelConditionView.LevelID;
                    request.RowVersion = levelConditionDetailView.LevelConditionView.RowVersion;

                    GeneralResponse response = this._levelConditionService.EditLevelCondition(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(levelConditionDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(levelConditionDetailView);
                }

            return View(levelConditionDetailView);
        }

        public ActionResult Details(string id)
        {
            LevelConditionDetailView levelConditionDetailView = new LevelConditionDetailView();
            
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("LevelCondition_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(levelConditionDetailView);
            }
            #endregion

            LevelConditionView levelConditionView = this.GetLevelConditionView(id);
            levelConditionDetailView.LevelConditionView = levelConditionView;
            // levelConditionDetailView.EmployeeView = GetEmployee();

            return View(levelConditionDetailView);
        }

        public ActionResult Delete(string id)
        {
            LevelConditionDetailView levelConditionDetailView = new LevelConditionDetailView();
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("LevelCondition_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(levelConditionDetailView);
            }
            #endregion

            levelConditionDetailView.LevelConditionView = this.GetLevelConditionView(id);
            //levelConditionDetailView.EmployeeView = GetEmployee();

            return View(levelConditionDetailView);
        }

        #endregion

        #region Private Members

        private LevelConditionView GetLevelConditionView(string id)
        {
            GetRequest2 request = new GetRequest2();
            request.ID1 = Guid.Parse(id);

            GetLevelConditionResponse response = this._levelConditionService.GetLevelCondition(request);

            return response.LevelConditionView;
        }

        #endregion

    }
}
