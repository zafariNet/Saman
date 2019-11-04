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
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class SuctionModeController: BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly ISuctionModeService _suctionModeService;

        #endregion

        #region Ctor

        public SuctionModeController(IEmployeeService employeeService, ISuctionModeService suctionModeService)
            : base(employeeService)
        {
            this._suctionModeService = suctionModeService;
            this._employeeService = employeeService;
        }

        #endregion

        #region Old Methods
        public ActionResult Index()
        {
            SuctionModeHomePageView suctionModeHomePageView = new SuctionModeHomePageView();
            suctionModeHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(suctionModeHomePageView);
            }
            #endregion

            suctionModeHomePageView.SuctionModeViews = this._suctionModeService.GetSuctionModes().SuctionModeViews;

            return View(suctionModeHomePageView);
        }

        #region Moving Up and Down
        public ActionResult MoveUp(string id)
        {
            MoveRequest request = new MoveRequest();
            request.ID = Guid.Parse(id);

            MoveResponse response = _suctionModeService.MoveUp(request);

            return RedirectToAction("Index");
        }

        public ActionResult MoveDown(string id)
        {
            MoveRequest request = new MoveRequest();
            request.ID = Guid.Parse(id);

            MoveResponse response = _suctionModeService.MoveDown(request);

            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult Create()
        {
            SuctionModeDetailView suctionModeDetailView = new SuctionModeDetailView();
            suctionModeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(suctionModeDetailView);
            }
            #endregion
            
            return View(suctionModeDetailView);
        }

        [HttpPost]
        public ActionResult Create(SuctionModeDetailView suctionModeDetailView)
        {
            suctionModeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(suctionModeDetailView);
            }
            #endregion
            
            if (ModelState.IsValid)
                try
                {
                    AddSuctionModeRequestOld request = new AddSuctionModeRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.SuctionModeName = suctionModeDetailView.SuctionModeView.SuctionModeName;

                    GeneralResponse response = this._suctionModeService.AddSuctionMode(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(suctionModeDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(suctionModeDetailView);
                }

            return View(suctionModeDetailView);
        }

        public ActionResult Edit(string id)
        {
            SuctionModeDetailView suctionModeDetailView = new SuctionModeDetailView();
            suctionModeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(suctionModeDetailView);
            }
            #endregion
            

            suctionModeDetailView.SuctionModeView = this.GetSuctionModeView(id);
            

            return View(suctionModeDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, SuctionModeDetailView suctionModeDetailView)
        {
            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(suctionModeDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditSuctionModeRequestOld request = new EditSuctionModeRequestOld();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.SuctionModeName = suctionModeDetailView.SuctionModeView.SuctionModeName;
                    request.RowVersion = suctionModeDetailView.SuctionModeView.RowVersion;

                    GeneralResponse response = this._suctionModeService.EditSuctionMode(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(suctionModeDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(suctionModeDetailView);
                }

            return View(suctionModeDetailView);
        }

        public ActionResult Details(string id)
        {
            SuctionModeDetailView suctionModeDetailView = new SuctionModeDetailView();
            suctionModeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(suctionModeDetailView);
            }
            #endregion

            SuctionModeView suctionModeView = this.GetSuctionModeView(id);

            
            suctionModeDetailView.SuctionModeView = suctionModeView;
            
            return View(suctionModeDetailView);
        }

        public ActionResult Delete(string id)
        {
            SuctionModeDetailView suctionModeDetailView = new SuctionModeDetailView();
            suctionModeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(suctionModeDetailView);
            }
            #endregion

            suctionModeDetailView.SuctionModeView = this.GetSuctionModeView(id);
            
            return View(suctionModeDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            SuctionModeDetailView suctionModeDetailView = new SuctionModeDetailView();


            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(suctionModeDetailView);
            }
            #endregion


            suctionModeDetailView.SuctionModeView = this.GetSuctionModeView(id);
            //suctionModeDetailView.EmployeeView = GetEmployee();

            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._suctionModeService.DeleteSuctionMode(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(suctionModeDetailView);
            }
        }

        #endregion

        #region New Methods

        #region Read

        public JsonResult SuctionModes_Read(int? pageSize, int? pageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<SuctionModeView>> response=new GetGeneralResponse<IEnumerable<SuctionModeView>>();
            
            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _suctionModeService.Get_SuctionModes(PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SuctionModes_Read_NoPermission(int? pageSize, int? pageNumber, string sort)
        {
            GetGeneralResponse<IEnumerable<SuctionModeView>> response = new GetGeneralResponse<IEnumerable<SuctionModeView>>();

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _suctionModeService.Get_SuctionModes();

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Insert

        public JsonResult SuctionModes_Insert(IEnumerable<AddSuctionModeRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            
            #endregion

            response = _suctionModeService.AddSuctionModes(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update

        public JsonResult SuctionModes_Update(IEnumerable<EditSuctionModeRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _suctionModeService.EditSuctionModes(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Delete

        public JsonResult SuctionModes_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _suctionModeService.DeleteSuctionModes(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Moveing

        public JsonResult SuctionModeDetail_MoveUp(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _suctionModeService.MoveUp(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SuctionModeDetail_MoveDown(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _suctionModeService.MoveDown(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Private Members

        private SuctionModeView GetSuctionModeView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetSuctionModeResponse response = this._suctionModeService.GetSuctionMode(request);

            return response.SuctionModeView;
        }

        #endregion

    }
}
