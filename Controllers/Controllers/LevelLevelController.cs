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

namespace Controllers.Controllers
{
    [Authorize]
    public class LevelLevelController : BaseController
    {
        private readonly IEmployeeService _employeeService;

        private readonly ILevelLevelService _levelLevelService;

        public LevelLevelController(IEmployeeService employeeService, ILevelLevelService levelLevelService)
            : base(employeeService)
        {
            this._levelLevelService = levelLevelService;
            this._employeeService = employeeService;
        }

        public ActionResult Index()
        {
            LevelLevelHomePageView levelLevelHomePageView = new LevelLevelHomePageView();
            levelLevelHomePageView.EmployeeView = GetEmployee();

            levelLevelHomePageView.LevelLevelViews = this._levelLevelService.GetLevelLevels().data;

            return View(levelLevelHomePageView);
        }

        public ActionResult Create()
        {
            LevelLevelDetailView levelLevelDetailView = new LevelLevelDetailView();
            levelLevelDetailView.EmployeeView = GetEmployee();

            return View(levelLevelDetailView);
        }

        [HttpPost]
        public ActionResult Create(LevelLevelDetailView levelLevelDetailView)
        {
            if (ModelState.IsValid)
                try
                {
                    AddLevelLevelRequest request = new AddLevelLevelRequest();
                    request.LevelID = levelLevelDetailView.LevelLevelView.LevelID;
                    request.NextLevelID = levelLevelDetailView.LevelLevelView.RelatedLevelID;

                    GeneralResponse response = this._levelLevelService.AddLevelLevel(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(levelLevelDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(levelLevelDetailView);
                }

            return View(levelLevelDetailView);
        }

        public ActionResult Edit(string id)
        {
            LevelLevelDetailView levelLevelDetailView = new LevelLevelDetailView();
            levelLevelDetailView.LevelLevelView = this.GetLevelLevelView(id);
            //levelLevelDetailView.EmployeeView = GetEmployee();

            return View(levelLevelDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, LevelLevelDetailView levelLevelDetailView)
        {
            if (ModelState.IsValid)
                try
                {
                    EditLevelLevelRequest request = new EditLevelLevelRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID; request.LevelID = levelLevelDetailView.LevelLevelView.LevelID;
                    request.NextLevelID = levelLevelDetailView.LevelLevelView.RelatedLevelID;
                    request.RowVersion = levelLevelDetailView.LevelLevelView.RowVersion;

                    GeneralResponse response = this._levelLevelService.EditLevelLevel(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(levelLevelDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(levelLevelDetailView);
                }

            return View(levelLevelDetailView);
        }

        public ActionResult Details(string id)
        {
            LevelLevelView levelLevelView = this.GetLevelLevelView(id);

            LevelLevelDetailView levelLevelDetailView = new LevelLevelDetailView();
            levelLevelDetailView.LevelLevelView = levelLevelView;
            // levelLevelDetailView.EmployeeView = GetEmployee();

            return View(levelLevelDetailView);
        }

        public ActionResult Delete(string id)
        {
            LevelLevelDetailView levelLevelDetailView = new LevelLevelDetailView();
            levelLevelDetailView.LevelLevelView = this.GetLevelLevelView(id);
            //levelLevelDetailView.EmployeeView = GetEmployee();

            return View(levelLevelDetailView);
        }

        //[HttpPost]
        //public ActionResult Delete(string id, FormCollection collection)
        //{
        //    LevelLevelDetailView levelLevelDetailView = new LevelLevelDetailView();
        //    levelLevelDetailView.LevelLevelView = this.GetLevelLevelView(id);
        //    //levelLevelDetailView.EmployeeView = GetEmployee();
        //    
        //    DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

        //    DeleteResponse response = this._levelLevelService.DeleteLevelLevel(request);

        //    if (response.hasCenter)
        //        return RedirectToAction("Index");
        //    else
        //    {
        //        foreach (string error in response.ErrorMessages)
        //            ModelState.AddModelError("", error);
        //        return View(levelLevelDetailView);
        //    }
        //}

        #region Private Members

        private LevelLevelView GetLevelLevelView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetLevelLevelResponse response = this._levelLevelService.GetLevelLevel(request);

            return response.LevelLevelView;
        }

        #endregion

    }
}
