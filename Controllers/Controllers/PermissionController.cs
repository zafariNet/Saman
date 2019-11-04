using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.EmployeeCatalog;
using System.Web.Mvc;
//using System.Web.UI.WebControls;
using Services.ViewModels.Employees;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;

namespace Controllers.Controllers
{
    [Authorize]
    public class PermissionController : BaseController
    {
        private readonly IEmployeeService _employeeService;

        private readonly IPermissionService _permissionService;

        public PermissionController(IEmployeeService employeeService, IPermissionService permissionService)
            : base(employeeService)
        {
            this._permissionService = permissionService;
            this._employeeService = employeeService;
        }

        public ActionResult Index()
        {
            PermissionHomePageView permissionHomePageView = new PermissionHomePageView();
            permissionHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Permission_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(permissionHomePageView);
            }
            #endregion

            permissionHomePageView.PermissionViews = this._permissionService.GetPermissions().PermissionViews;

            return View(permissionHomePageView);
        }

        public ActionResult Create()
        {
            PermissionDetailView permissionDetailView = new PermissionDetailView();
            permissionDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Permission_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(permissionDetailView);
            }
            #endregion

            return View(permissionDetailView);
        }

        [HttpPost]
        public ActionResult Create(PermissionDetailView permissionDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Permission_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(permissionDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddPermissionRequest request = new AddPermissionRequest();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.Group = permissionDetailView.PermissionView.Group;
                    request.Key = permissionDetailView.PermissionView.Key;
                    request.Title = permissionDetailView.PermissionView.Title;

                    GeneralResponse response = this._permissionService.AddPermission(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(permissionDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(permissionDetailView);
                }

            return View(permissionDetailView);
        }

        public ActionResult Edit(string id)
        {
            PermissionDetailView permissionDetailView = new PermissionDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Permission_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                //return View(permissionDetailView);
            }
            #endregion
            
            permissionDetailView.PermissionView = this.GetPermissionView(id);
            //permissionDetailView.EmployeeView = GetEmployee();

            return View(permissionDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, PermissionDetailView permissionDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Permission_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(permissionDetailView);
            }
            #endregion

            //if (ModelState.IsValid)
            //    try
            //    {
            //        EditPermissionRequest request = new EditPermissionRequest();

            //        request.ID = Guid.Parse(id);
            //        request.ModifiedEmployeeID = GetEmployee().ID;
            //        //request.Group = permissionDetailView.PermissionView.Group;
            //        request.Key = permissionDetailView.PermissionView.Key;
            //        request.Title = permissionDetailView.PermissionView.Title;
            //        request.RowVersion = permissionDetailView.PermissionView.RowVersion;

            //        EditResponse response = this._permissionService.EditPermission(request);

            //        if (response.hasCenter)
            //            return RedirectToAction("Index");
            //        else
            //        {
            //            foreach (string error in response.ErrorMessages)
            //                ModelState.AddModelError("", error);
            //            return View(permissionDetailView);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        ModelState.AddModelError("", ex.Message);
            //        return View(permissionDetailView);
            //    }

            return View(permissionDetailView);
        }

        public ActionResult Details(string id)
        {
            PermissionDetailView permissionDetailView = new PermissionDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Permission_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(permissionDetailView);
            }
            #endregion

            PermissionView permissionView = this.GetPermissionView(id);

            permissionDetailView.PermissionView = permissionView;
            // permissionDetailView.EmployeeView = GetEmployee();

            return View(permissionDetailView);
        }

        public ActionResult Delete(string id)
        {
            PermissionDetailView permissionDetailView = new PermissionDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Permission_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(permissionDetailView);
            }
            #endregion

            permissionDetailView.PermissionView = this.GetPermissionView(id);
            //permissionDetailView.EmployeeView = GetEmployee();

            return View(permissionDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            PermissionDetailView permissionDetailView = new PermissionDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Permission_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(permissionDetailView);
            }
            #endregion

            
            permissionDetailView.PermissionView = this.GetPermissionView(id);
            //permissionDetailView.EmployeeView = GetEmployee();
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._permissionService.DeletePermission(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(permissionDetailView);
            }
        }

        #region Private Members

        private PermissionView GetPermissionView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetPermissionResponse response = this._permissionService.GetPermission(request);

            return response.PermissionView;
        }

        #endregion

    }
}
