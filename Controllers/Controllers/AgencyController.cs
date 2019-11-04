#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Controllers.ViewModels.CustomerCatalog;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Customers;
using Infrastructure.Querying;
using System.Web.Script.Serialization;


#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class AgencyController : BaseController
    {
        #region Delcares

        private readonly IEmployeeService _employeeService;

        private readonly IAgencyService _agencyService;

        private readonly IProvinceService _provinceService;

        #endregion

        #region Ctor
        public AgencyController(IEmployeeService employeeService, IAgencyService agencyService, IProvinceService provinceService)
            : base(employeeService)
        {
            this._agencyService = agencyService;
            this._employeeService = employeeService;
            this._provinceService = provinceService;
        }
        #endregion

        #region Old Methods

        public JsonResult Provinces_Read(int? pageSize , int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<ProvinceView>> response = _provinceService.GetProvinces(-1, -1);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        


        #region Ajax Read
        
        public ActionResult Agency_Read([DataSourceRequest] DataSourceRequest request)
        {
            AgencyHomePageView agencyHomePageView = new AgencyHomePageView();
            agencyHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Read");
            if (!hasPermission)
            {
                return Json(agencyHomePageView.AgencyViews.ToDataSourceResult(request));
            }
            #endregion
            
            agencyHomePageView.AgencyViews = this._agencyService.GetAgencies().data;
            return Json(agencyHomePageView.AgencyViews.ToDataSourceResult(request));
        }

        #endregion

        #region Index
        public ActionResult Index()
        {
            AgencyHomePageView agencyHomePageView = new AgencyHomePageView();
            agencyHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermision = GetEmployee().IsGuaranteed("Agency_Read");
            if (!hasPermision)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(agencyHomePageView);
            }
            #endregion
            
            agencyHomePageView.AgencyViews = this._agencyService.GetAgencies().data;
            return View(agencyHomePageView);
        }
        #endregion

        #region Moving Up and Down
        public ActionResult MoveUp(string id)
        {
            MoveRequest request = new MoveRequest();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Update");
            if (!hasPermission)
            {
                return RedirectToAction("Index");
            }
            #endregion

            request.ID = Guid.Parse(id);

            MoveResponse response = _agencyService.MoveUp(request);

            return RedirectToAction("Index");
        }

        public ActionResult MoveDown(string id)
        {
            MoveRequest request = new MoveRequest();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Update");
            if (!hasPermission)
            {
                return RedirectToAction("Index");
            }
            #endregion

            request.ID = Guid.Parse(id);

            MoveResponse response = _agencyService.MoveDown(request);

            return RedirectToAction("Index");
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            AgencyDetailView agencyDetailView = new AgencyDetailView();
            agencyDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return View(agencyDetailView);
            }
            #endregion

            return View(agencyDetailView);
        }

        [HttpPost]
        public ActionResult Create(AgencyDetailView agencyDetailView)
        {
            if (ModelState.IsValid)
                try
                {
                    GeneralResponse response = new GeneralResponse();

                    #region Check Access
                    bool hasPermission = GetEmployee().IsGuaranteed("Agency_Insert");
                    if (!hasPermission)
                    {
                        ModelState.AddModelError("", "AccessDenied");
                        return View(agencyDetailView);
                    }
                    #endregion

                    AddAgencyRequestOld request = new AddAgencyRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.Address = agencyDetailView.AgencyView.Address;
                    request.AgencyName = agencyDetailView.AgencyView.AgencyName;
                    request.Discontinued = agencyDetailView.AgencyView.Discontinued;
                    request.ManagerName = agencyDetailView.AgencyView.ManagerName;
                    request.Mobile = agencyDetailView.AgencyView.Mobile;
                    request.Note = agencyDetailView.AgencyView.Note;
                    request.Phone1 = agencyDetailView.AgencyView.Phone1;
                    request.Phone2 = agencyDetailView.AgencyView.Phone2;

                    response = this._agencyService.AddAgency(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(agencyDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(agencyDetailView);
                }

            return View(agencyDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            AgencyDetailView agencyDetailView = new AgencyDetailView();
            agencyDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return View(agencyDetailView);
            }
            #endregion

            agencyDetailView.AgencyView = this.GetAgencyView(id);
            
            return View(agencyDetailView);
        }


        [HttpPost]
        public ActionResult Edit(string id, AgencyDetailView agencyDetailView)
        {
            agencyDetailView.EmployeeView = GetEmployee();

            #region Check Access
            GeneralResponse response = new GeneralResponse();
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied"); ;
                return View(agencyDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditAgencyRequestOld request = new EditAgencyRequestOld();

                    

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.Address = agencyDetailView.AgencyView.Address;
                    request.AgencyName = agencyDetailView.AgencyView.AgencyName;
                    request.Discontinued = agencyDetailView.AgencyView.Discontinued;
                    request.ManagerName = agencyDetailView.AgencyView.ManagerName;
                    request.Mobile = agencyDetailView.AgencyView.Mobile;
                    request.Note = agencyDetailView.AgencyView.Note;
                    request.Phone1 = agencyDetailView.AgencyView.Phone1;
                    request.Phone2 = agencyDetailView.AgencyView.Phone2;
                    request.RowVersion = agencyDetailView.AgencyView.RowVersion;

                    response = this._agencyService.EditAgency(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(agencyDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(agencyDetailView);
                }

            return View(agencyDetailView);
        }
        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            AgencyDetailView agencyDetailView = new AgencyDetailView();
            agencyDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return View(agencyDetailView);
            }
            
            #endregion

            AgencyView agencyView = this.GetAgencyView(id);
            
            agencyDetailView.AgencyView = agencyView;

            return View(agencyDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            AgencyDetailView agencyDetailView = new AgencyDetailView();
            agencyDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(agencyDetailView);
            }
            #endregion

            agencyDetailView.AgencyView = this.GetAgencyView(id);
            
            return View(agencyDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            AgencyDetailView agencyDetailView = new AgencyDetailView();
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return View(agencyDetailView);
            }
            #endregion

            agencyDetailView.AgencyView = this.GetAgencyView(id);
            agencyDetailView.EmployeeView = GetEmployee();

            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            response = this._agencyService.DeleteAgency(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(agencyDetailView);
            }
        }
        #endregion

        #endregion

        #region  New Methods

        #region Read

        public JsonResult Agencies_Read(bool? Discontinued,int? pageSize, int? pageNumber,string sort, IList<FilterData> filter)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            
            response = _agencyService.GetAgencies(Discontinued, PageSize, PageNumber,ConvertJsonToObject(sort),filter);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ActiveAgencies_Read()
        {
            GetGeneralResponse<IEnumerable<AgencyView>> response=new GetGeneralResponse<IEnumerable<AgencyView>>();

            response = _agencyService.GetActiveAgencies();

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Insert

        public JsonResult Agencies_Insert(AddAgencyRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = this._agencyService.AddAgency(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult Agency_Update(EditAgencyRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = this._agencyService.EditAgency(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Agencies_Update(IEnumerable<EditAgencyRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = this._agencyService.EditAgencies(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult Agencies_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _agencyService.DeleteAgency(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Moveing

        public JsonResult Agency_MoveUp(Guid ID)
        {
            GeneralResponse response=new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _agencyService.MoveUp(new MoveRequest() { ID=ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Agency_MoveDown(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _agencyService.MoveDown(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Private Members

        private AgencyView GetAgencyView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetAgencyResponse response = this._agencyService.GetAgency(request);

            return response.AgencyView;
        }

        #endregion

        #region Grid

        public ActionResult GridView()
        {

            AgencyHomePageView agencyHomePageView = new AgencyHomePageView();
            agencyHomePageView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Agency_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return View(agencyHomePageView);
            }
            #endregion

            agencyHomePageView.AgencyViews = this._agencyService.GetAgencies().data;

            return View(agencyHomePageView);

        }

        #endregion

    }
}
